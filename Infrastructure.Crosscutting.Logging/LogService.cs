using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Compilation;
using Domain.Contracts.Infrastructure.Crosscutting;
using Infrastructure.Crosscutting.Shared.RestClient;

namespace Infrastructure.Crosscutting.Logging
{
    public class LogService : ILogService
    {
        private readonly IInventAppRestClient _restClient;

        private readonly string _application;
        private readonly string _projectName;
        private readonly Guid _correlationId;

        public LogService()
        {
            _application = "InventApp";
            _projectName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First();

            const string project = "logging";

            var envUrl = new Dictionary<string, string>
            {
                { "DEV",   $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "TEST",  $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "STAGE", $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api" },
                { "PROD",  $"http://www.ucirod.infrastructure.com:40000/{project}/api" }
            };

            _restClient = new InventAppRestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);

            var request = new InventAppRestRequest
            {
                Resource = "correlations",
                Method = InventAppRestMethod.POST
            };
            var correlationResponse = _restClient.Post<Correlation>(request);

            _correlationId = correlationResponse.Data.Id;
        }

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
            var task = new Task(() =>
            {
                try
                {
                    var request = new InventAppRestRequest
                    {
                        Resource = "logs",
                        Method = InventAppRestMethod.POST,
                        JsonBody = new Log(_application, _projectName, _correlationId, messageToLog, logType)
                    };

                    _restClient.Post(request);
                }
                catch (Exception e) 
                {
                    //TODO: log locally in somewhere when the connection with Log Micro-Service fails (example: local file, local db, iis)
                    Console.WriteLine(e);
                }
            });

            task.Start();
        }
    }
}
