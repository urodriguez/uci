using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.DequeueResolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using Newtonsoft.Json;

namespace Infrastructure.Crosscutting.Logging
{
    public class LogService : AsyncInfrastractureService, ILogService, ILogDequeueResolver
    {
        private readonly IAppSettingsService _appSettingsService;

        private string _correlationId;

        private static readonly object Locker = new Object();

        public LogService(IAppSettingsService appSettingsService, IEnqueueService queueService) : base(queueService)
        {
            _appSettingsService = appSettingsService;

            UseLogger(this);
            UseBaseUrl(appSettingsService.LoggingApiUrlV1);

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

        private void LogMessage(string messageToLog, LogType logType)
        {
            var log = new Log(
                _appSettingsService.InfrastructureCredential,
                "InventApp",
                BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First(),
                _correlationId,
                messageToLog,
                logType,
                _appSettingsService.Environment.Name
            );

            ExecuteAsync("logs", log);
        }

        public void Log(Log log)
        {
            log.Credential = _appSettingsService.InfrastructureCredential;
            ExecuteAsync("logs", log);
        }

        public void DeleteOldLogs()
        {
            //Remove logs from file system
            try
            {
                LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Deleting Old Logs From FileSystem");

                foreach (var fileSystemLogsDirectory in Directory.GetDirectories(_appSettingsService.FileSystemLogsDirectory))
                {
                    var directoryInfo = new DirectoryInfo(fileSystemLogsDirectory);
                    LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Processing directory | directory.Name={directoryInfo.Name}");

                    var filesToDelete = directoryInfo.GetFiles("*.txt").Where(f => f.CreationTime < DateTime.Today.AddDays(-7));
                    var filesDeleted = 0;
                    foreach (var fileToDelete in filesToDelete)
                    {
                        fileToDelete.Delete();
                        filesDeleted++;
                    }

                    LogInfoMessage(
                        filesToDelete.Count() != filesDeleted ?
                            $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Directory processed | status=INCOMPLED - directory.Name={directoryInfo.Name} - filesToDelete={filesToDelete.Count()} - filesDeleted={filesDeleted}"
                            :
                            $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Directory processed | status=FINISHED - directory.Name={directoryInfo.Name} - filesToDelete={filesToDelete.Count()} - filesDeleted={filesDeleted}"
                        );
                }
            }
            catch (Exception e)
            {
                LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | FileSystem Exception | e={e}");
            }
        }

        public void ResetCorrelationId()
        {
            _correlationId = Guid.NewGuid().ToString();
        }

        public void FileSystemLog(string messageToLog)
        {
            var projectName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First();
            var projFileSystemLogsDirectory = $"{_appSettingsService.FileSystemLogsDirectory}\\{projectName}";
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

        public void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData)
        {
            foreach (var queueItemJsonData in queueItemsJsonData)
            {
                var log = JsonConvert.DeserializeObject<Log>(queueItemJsonData);
                Log(log);
            }
        }

        public bool ResolvesQueueItemType(QueueItemType queueItemType) => queueItemType == QueueItemType.Log;
    }
}
