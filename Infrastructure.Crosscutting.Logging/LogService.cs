using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Compilation;
using RestSharp;

namespace Infrastructure.Crosscutting.Logging
{
    public class LogService : ILogService
    {
        private readonly QueryFormatter _queryFormatter;

        private readonly IRestClient _restClient;

        private readonly string _application;
        private readonly string _projectName;
        private readonly Guid _correlationId;

        public LogService()
        {
            _queryFormatter = new QueryFormatter();

            _application = "InventApp";
            _projectName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First();

            var envUrl = new Dictionary<string, string>
            {
                { "DEV", "http://www.ucirod.test-logging.com:40000/api" },
                { "TEST", "http://www.ucirod.test-logging.com:40000/api" },
                { "STAGE", "http://www.ucirod.stage-logging.com/api" },
                { "PROD", "http://www.ucirod.logging.com/api" }
            };

            _restClient = new RestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);

            var request = new RestRequest("correlations", Method.POST);
            var correlationResponse = _restClient.Post<Correlation>(request);

            _correlationId = correlationResponse.Data.Id;
        }

        //Very detailed logs, which may include high-volume information such as protocol payloads. This log level is typically only enabled during development
        public void LogTraceMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            LogMessage(messageToLog, LogType.Trace, messageType);
        }

        //Information messages, which are normally enabled in production environment
        public void LogInfoMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            LogMessage(messageToLog, LogType.Info, messageType);
        }

        //Error messages - most of the time these are Exceptions
        public void LogErrorMessage(string messageToLog, MessageType messageType = MessageType.Text)
        {
            LogMessage(messageToLog, LogType.Error, messageType);
        }

        private void LogMessage(string messageToLog, LogType logType, MessageType messageType)
        {
            var task = new Task(() =>
            {
                var request = new RestRequest("logs", Method.POST);

                var log = new Log(_application, _projectName, _correlationId, messageType == MessageType.Text ? messageToLog : _queryFormatter.Format(messageToLog), logType);
                request.AddJsonBody(log);

                _restClient.Post(request);
            });

            task.Start();
        }
    }
}
