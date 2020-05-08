﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Queueing;

namespace Infrastructure.Crosscutting.Logging
{
    public class LogService : AsyncInfrastractureService, ILogService
    {
        private readonly IAppSettingsService _appSettingsService;

        private readonly string _correlationId;

        private static readonly object Locker = new Object();

        public LogService(IAppSettingsService appSettingsService, IQueueService queueService) : base(queueService)
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

        public void DeleteOldLogs()
        {
            //Remove logs from file system
            try
            {
                LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Deleting Old Logs From FS");

                foreach (var fileSystemLogsDirectory in Directory.GetDirectories(_appSettingsService.FileSystemLogsDirectory))
                {
                    var directoryInfo = new DirectoryInfo(fileSystemLogsDirectory);
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
            }
            catch (Exception e)
            {
                LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Logging FS Exception | e={e}");
            }
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
    }
}
