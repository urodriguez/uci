using System.Collections.Generic;
using NLog;

namespace Infrastructure.Crosscutting.Logging
{
    //https://github.com/NLog/NLog/wiki/Tutorial
    public class NLogService : ILogService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly QueryFormatter _queryFormatter;
        private IList<LogMessage> _logMessagesQueued;

        public NLogService()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "Logs/file.txt" };

            // Rules for mapping loggers to targets           
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            _queryFormatter = new QueryFormatter();
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

        public void QueueTraceMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            switch (messageType)
            {
                case MessageType.Text:
                    _logMessagesQueued.Add(new LogMessage(messageToLog, LogLevel.Trace));
                    break;

                case MessageType.Query:
                    _logMessagesQueued.Add(new LogMessage(_queryFormatter.Format(messageToLog), LogLevel.Trace));
                    break;
            }
        }

        public void QueueErrorMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            switch (messageType)
            {
                case MessageType.Text:
                    _logMessagesQueued.Add(new LogMessage(messageToLog, LogLevel.Error));
                    break;

                case MessageType.Query:
                    _logMessagesQueued.Add(new LogMessage(_queryFormatter.Format(messageToLog), LogLevel.Error));
                    break;
            }
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
            _logMessagesQueued.Clear();
        }
    }
}
