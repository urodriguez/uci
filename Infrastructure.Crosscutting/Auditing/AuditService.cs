using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Contracts.Infrastructure.Crosscutting.Auditing;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;
using RestSharp;

namespace Infrastructure.Crosscutting.Auditing
{
    public class AuditService : IAuditService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;

        public AuditService(ILogService logService)
        {
            _logService = logService;

            const string project = "auditing";

            var envUrl = new Dictionary<string, string>
            {
                { "DEV",   $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "TEST",  $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "STAGE", $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api" },
                { "PROD",  $"http://www.ucirod.infrastructure.com:40000/{project}/api" }
            };

            _restClient = new RestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);
        }

        //TODO: implement avoid lost audit if connection fails
        public void Audit(IAuditable auditable)
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
                    request.AddJsonBody(auditable);

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
                    _logService.LogErrorMessage($"{GetType().Name}.{methodName} | An error has occurred serializing | id={auditable.EntityName} - action={auditable.Action}");
                    _logService.LogErrorMessage($"{GetType().Name}.{methodName} | errorMessage={e.Message}");
                }
            });

            task.Start();
        }
    }
}
