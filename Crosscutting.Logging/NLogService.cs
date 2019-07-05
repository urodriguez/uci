using System;
using System.Collections.Generic;
using NLog;

namespace Crosscutting.Logging
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

        public void Log(LogMessage logMessage)
        {
            switch (logMessage.LogLevel)
            {
                case LogLevel.Trace:
                    Logger.Trace(logMessage.Message);
                    break;

                case LogLevel.Error:
                    Logger.Error(logMessage.Message);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void QueueLogMessage(LogMessage logMessage)
        {
            _logMessagesQueued.Add(logMessage);
        }

        public void FlushQueueLogMessages()
        {
            foreach (var logMessageQueued in _logMessagesQueued)
            {
                Log(logMessageQueued);
            }
        }
    }
}
