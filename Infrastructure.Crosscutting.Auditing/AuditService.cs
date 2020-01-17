using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Enums;
using Newtonsoft.Json;
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

            var envUrl = new Dictionary<string, string> //TODO: use correct urls
            {
                { "DEV", "http://www.ucirod.infrastructure-test.com:40000/Auditing/api" },
                { "TEST", "http://www.ucirod.infrastructure-test.com:40000/Auditing/api" },
                { "STAGE", "http://www.ucirod.infrastructure-stage.com:40000/Auditing/api" },
                { "PROD", "http://www.ucirod.infrastructure.com:40000/Auditing/api" }
            };

            _restClient = new RestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);
        }

        //TODO: implement avoid lost audit if connection fails
        public void Audit(IAggregateRoot entity, AuditAction action)
        {
            var task = new Task(() =>
            {
                try
                {
                    _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Serializing entity | id={entity.Id} action={action}");

                    var entityJson = JsonConvert.SerializeObject(entity);

                    var audit = new AuditDto
                    {
                        Application = "InventApp",
                        Environment = ConfigurationManager.AppSettings["Environment"],
                        User = Thread.CurrentPrincipal.Identity.Name,
                        Entity = entityJson,
                        EntityName = entity.GetType().Name,
                        Action = action
                    };

                    var request = new RestRequest("audits", Method.POST);
                    request.AddJsonBody(audit);

                    _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending audit data to Audit Micro-service");
                    var response = _restClient.Post(request);
                    _logService.LogInfoMessage(response.IsSuccessful ? $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Audit data sent to Audit Micro-service | Status=OK" 
                                                                     : $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending audit data to Audit Micro-service | Status=FAIL - Reason={response.Content}");
                }
                catch (Exception e)
                {
                    _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | An error has occurred serializing | id={entity.Id} - action={action}");
                    _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | errorMessage={e.Message}");
                }
            });

            task.Start();
        }
    }
}
