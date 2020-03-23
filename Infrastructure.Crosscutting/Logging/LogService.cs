using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Compilation;
using Infrastructure.Crosscutting.AppSettings;
using RestSharp;

namespace Infrastructure.Crosscutting.Logging
{
    public class LogService : ILogService
    {
        private readonly IRestClient _restClient;

        private readonly string _application;
        private readonly string _projectName;
        private readonly Guid _correlationId;

        public LogService(IAppSettingsService appSettingsService)
        {
            _application = "InventApp";
            _projectName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First();

            _restClient = new RestClient(appSettingsService.LoggingUrl);

            var request = new RestRequest
            {
                Resource = "correlations",
                Method = Method.POST
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
                    var request = new RestRequest
                    {
                        Resource = "logs",
                        Method = Method.POST
                    };
                    request.AddJsonBody(new Log(_application, _projectName, _correlationId, messageToLog, logType));

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
