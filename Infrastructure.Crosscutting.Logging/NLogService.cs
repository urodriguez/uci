using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;

namespace Infrastructure.Crosscutting.Logging
{
    //https://github.com/NLog/NLog/wiki/Tutorial
    public class NLogService : ILogService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly QueryFormatter _queryFormatter;
        private readonly IList<LogMessage> _logMessagesQueued;
        private readonly Guid _correlationId;

        public NLogService()
        {
            #region NLOG_CONFIG
            var config = new LoggingConfiguration();

            LayoutRenderer.Register("projectName", logEvent => BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First());

            // Rules for mapping loggers to targets           
            const string layout = "${longdate} | ${projectName}${newline}${message}";
            var logsBaseDir = $"Logs/{DateTime.Now:MM-dd-yyyy}";
            config.AddRuleForOneLevel(NLog.LogLevel.Trace, new FileTarget("logfile") { FileName = $"{logsBaseDir}/trace.txt", Layout = layout });
            config.AddRuleForOneLevel(NLog.LogLevel.Info, new FileTarget("logfile")  { FileName = $"{logsBaseDir}/info.txt", Layout = layout });
            config.AddRuleForOneLevel(NLog.LogLevel.Error, new FileTarget("logfile") { FileName = $"{logsBaseDir}/error.txt", Layout = layout });

            // Apply config           
            LogManager.Configuration = config;
            #endregion

            _queryFormatter = new QueryFormatter();
            _logMessagesQueued = new List<LogMessage>();
            _correlationId = Guid.NewGuid();
        }

        //Very detailed logs, which may include high-volume information such as protocol payloads. This log level is typically only enabled during development
        public void QueueTraceMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            QueueMessage(messageToLog, LogType.Trace, messageType);
        }

        //Information messages, which are normally enabled in production environment
        public void QueueInfoMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            QueueMessage(messageToLog, LogType.Info, messageType);
        }

        //Error messages - most of the time these are Exceptions
        public void QueueErrorMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            QueueMessage(messageToLog, LogType.Error, messageType);
        }

        private void QueueMessage(string messageToLog, LogType logType, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Text:
                    _logMessagesQueued.Add(new LogMessage(messageToLog, logType));
                    break;

                case MessageType.Query:
                    _logMessagesQueued.Add(new LogMessage(_queryFormatter.Format(messageToLog), logType));
                    break;
            }
        }

        public void FlushQueueMessages()
        {
            foreach (var logMessageQueued in _logMessagesQueued)
            {
                var messageToLog = $"CorrelationId={_correlationId}{Environment.NewLine}{logMessageQueued.Message}";

                switch (logMessageQueued.LogType)
                {
                    case LogType.Trace:
                        Logger.Trace(messageToLog + Environment.NewLine);
                        break;

                    case LogType.Info:
                        Logger.Info(messageToLog + Environment.NewLine);
                        break;

                    case LogType.Error:
                        Logger.Error(messageToLog + Environment.NewLine);
                        break;
                }
            }
            _logMessagesQueued.Clear();
        }
    }
}
