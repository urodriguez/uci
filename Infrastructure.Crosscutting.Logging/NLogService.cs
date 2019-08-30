using System;
using System.Collections.Generic;
using NLog;

namespace Infrastructure.Crosscutting.Logging
{
    //https://github.com/NLog/NLog/wiki/Tutorial
    public class NLogService : ILoggerService
    {
      private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IList<LogMessage> _logMessagesQueued;

        public NLogService()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "Logs/file.txt" };

            // Rules for mapping loggers to targets           
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            _logMessagesQueued = new List<LogMessage>();
        }

        public void LogTrace(string messageToLog)
        {
            Logger.Trace(messageToLog);
        }

        public void LogError(string messageToLog)
        {
            Logger.Error(messageToLog);
        }

        public void QueueMessageTrace(string messageToLog)
        {
            _logMessagesQueued.Add(new LogMessage(messageToLog, LogLevel.Trace));
        }

        public void QueueMessageError(string messageToLog)
        {
            _logMessagesQueued.Add(new LogMessage(messageToLog, LogLevel.Error));
        }

        public void FlushQueueMessages()
        {
            foreach (var logMessageQueued in _logMessagesQueued)
            {
                switch (logMessageQueued.LogLevel)
                {
                    case LogLevel.Trace:
                        Logger.Trace(logMessageQueued.Message);
                        break;

                    case LogLevel.Error:
                        Logger.Error(logMessageQueued.Message);
                        break;
                }
            }
        }
    }
}
