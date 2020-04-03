using System;
using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using RestSharp;

namespace Infrastructure.Crosscutting.Auditing
{
    public class AuditService : IAuditService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        public AuditService(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.AuditingApiUrl);
        }

        //TODO: implement avoid lost audit if connection fails
        public void Audit(Audit audit)
        {
            var methodName = MethodBase.GetCurrentMethod().Name;//Get method name outside task context

            var task = new Task(() =>
            {
                try
                {
                    var request = new RestRequest
                    {
                        Resource = "audits",
                        Method = Method.POST,
                    };
                    audit.Credential = _appSettingsService.InfrastructureCredential;
                    request.AddJsonBody(audit);

                    _logService.LogInfoMessage($"{GetType().Name}.{methodName} | Sending audit data to Audit Micro-service");

                    var response = _restClient.Post(request);

                    _logService.LogInfoMessage(
                        response.IsSuccessful 
                            ? $"{GetType().Name}.{methodName} | Audit data sent to Audit Micro-service | Status=OK" 
                            : $"{GetType().Name}.{methodName} | Error sending audit data to Audit Micro-service | Status=FAIL - Reason={response.Content}"
                    );
                }
                catch (Exception e)
                {
                    _logService.LogErrorMessage($"{GetType().Name}.{methodName} | An error has occurred serializing | id={audit.EntityName} - action={audit.Action}");
                    _logService.LogErrorMessage($"{GetType().Name}.{methodName} | errorMessage={e.Message}");
                }
            });

            task.Start();
        }
    }
}
