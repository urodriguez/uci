using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using Infrastructure.Crosscutting.AppSettings;
using RestSharp;

namespace Infrastructure.Crosscutting.Logging
{
    public class LogService : ILogService
    {
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        private readonly string _application;
        private readonly string _projectName;
        private readonly string _correlationId;

        private static readonly object Locker = new Object();

        public LogService(IAppSettingsService appSettingsService)
        {
            _application = "InventApp";
            _projectName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First();

            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.LoggingApiUrl);

            _correlationId = Guid.NewGuid().ToString();
        }

        public string GetCorrelationId() => _correlationId;

        //Very detailed logs, which may include high-volume information such as protocol payloads. This log level is typically only enabled during development
        public void LogTraceMessage(string messageToLog)
        {
            LogMessage(messageToLog, LogType.Trace);
        }

        //Information messages, which are normally enabled in production environment
        public void LogInfoMessage(string messageToLog)
        {
            LogMessage(messageToLog, LogType.Info);
        }

        //Error messages - most of the time these are Exceptions
        public void LogErrorMessage(string messageToLog)
        {
            LogMessage(messageToLog, LogType.Error);
        }

        public void DeleteOldLogs()
        {
            //Remove logs from file system
            try
            {
                LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Deleting Old Logs From FS | status=PENDING");

                var directoryInfo = new DirectoryInfo(_appSettingsService.FileSystemLogsDirectory);
                var filesToDelete = directoryInfo.GetFiles("*.txt").Where(f => f.CreationTime < DateTime.Today.AddDays(-7));
                var filesDeleted = 0;
                foreach (var fileToDelete in filesToDelete)
                {
                    fileToDelete.Delete();
                    filesDeleted++;
                }

                LogInfoMessage(
                    filesToDelete.Count() != filesDeleted ? 
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Deleting Old Logs From FS | status=INCOMPLED - filesToDelete={filesToDelete.Count()} - filesDeleted={filesDeleted}" 
                        : 
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Deleting Old Logs From FS | status=FINISHED - filesToDelete={filesToDelete.Count()} - filesDeleted={filesDeleted}"
                );
            }
            catch (Exception e)
            {
                LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Logging FS Exception | e={e}");
            }
        }

        private void LogMessage(string messageToLog, LogType logType)
        {
            var task = new Task(() =>
            {
                try
                {
                    var request = new RestRequest
                    {
                        Resource = "logs",
                        Method = Method.POST
                    };
                    request.AddJsonBody(new Log(
                        _appSettingsService.InfrastructureCredential, 
                        _application, 
                        _projectName,
                        _correlationId,
                        messageToLog, 
                        logType, 
                        _appSettingsService.Environment.Name)
                    );

                    var logResponse =  _restClient.Post(request);

                    if (!logResponse.IsSuccessful)
                        throw new Exception(
                            $"logResponse.IsSuccessful=false - logResponse.StatusCode={logResponse.StatusCode} - logResponse.Content={logResponse.Content}"
                        );
                }
                catch (Exception e) 
                {
                    //Do not call LogService to log this exception in order to avoid infinite loop
                    FileSystemLog($"{e}");

                    //queue 'log' data

                    //do not throw the exception in order to avoid finish the main request
                }
            });

            task.Start();
        }

        private void FileSystemLog(string messageToLog)
        {
            var projFileSystemLogsDirectory = $"{_appSettingsService.FileSystemLogsDirectory}";
            Directory.CreateDirectory(projFileSystemLogsDirectory);

            var logFileName = $"FSL,{_correlationId}";
            var logFilePath = $"{projFileSystemLogsDirectory}\\{logFileName}.txt";

            lock (Locker)
            {
                using (FileStream file = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(file, Encoding.Unicode))
                {
                    sw.WriteLine($"{messageToLog}{Environment.NewLine}----------------******----------------{Environment.NewLine}");
                }
            }
        }
    }
}
