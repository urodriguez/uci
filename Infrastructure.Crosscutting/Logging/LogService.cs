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
        private readonly IAppSettingsService _appSettingsService;

        private readonly string _application;
        private readonly string _projectName;
        private readonly Guid _correlationId;

        public LogService(IAppSettingsService appSettingsService)
        {
            _application = "InventApp";
            _projectName = BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName.Split(',').First();

            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.LoggingApiUrl);

            var request = new RestRequest
            {
                Resource = "correlations",
                Method = Method.POST
            };
            request.AddJsonBody(new
            {
                Id = _appSettingsService.InfrastructureCredential.Id,
                SecretKey = _appSettingsService.InfrastructureCredential.SecretKey
            });

            try
            {
                var correlationResponse = _restClient.Post<Correlation>(request);
                
                if (!correlationResponse.IsSuccessful) 
                    throw new Exception(
                        $"correlationResponse.IsSuccessful=false - correlationResponse.StatusCode={correlationResponse.StatusCode} - correlationResponse.Content={correlationResponse.Content}"
                    );

                _correlationId = correlationResponse.Data.Id;
            }
            catch (Exception e)
            {
                throw new CorrelationException(e.Message, e.StackTrace);
            }
        }

        public Guid GetCorrelationId() => _correlationId;

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

        //TODO: implement avoid lost logs if connection fails
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
                    //TODO: log error locally here
                    //queue
                    //do not throw the exception in order to avoid finish the main request
                }
            });

            task.Start();
        }
    }
}
