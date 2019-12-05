using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Domain.Contracts.Aggregates;
using Infrastructure.Crosscutting.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                { "DEV", "https://localhost:44387/api" },
                { "TEST", "http://www.ucirod.infrastructure.auditing-test.com:40000/api" },
                { "STAGE", "http://www.ucirod.infrastructure.auditing-test.com:40000/api" },
                { "PROD", "http://www.ucirod.infrastructure.auditing-test.com:40000/api" }
            };

            _restClient = new RestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);
        }

        //TODO: implement avoid lost audit if connection fails
        public void Audit(IAggregateRoot entity, AuditAction action, IAggregateRoot oldEntity = null)
        {
            var task = new Task(() =>
            {
                try
                {
                    _logService.LogInfoMessage($"AuditService.Audit - Serializing entity with id = {entity.Id} for audit action = {action}");

                    var entityJson = JsonConvert.SerializeObject(entity);
                    var oldEntityJson = oldEntity != null ? JsonConvert.SerializeObject(oldEntity) : null;

                    var audit = new AuditDto
                    {
                        Action = action,
                        Application = "InventApp",
                        Entity = entityJson,
                        OldEntity = oldEntityJson,
                        User = Thread.CurrentPrincipal.Identity.Name
                    };

                    var request = new RestRequest("audits", Method.POST);
                    request.AddJsonBody(audit);

                    _logService.LogInfoMessage("AuditService.Audit - Sending audit data to Audit Micro-service");
                    var response = _restClient.Post(request);
                    _logService.LogInfoMessage(response.IsSuccessful ? "AuditService.Audit - Audit data sent to Audit Micro-service - Status: OK" : $"AuditService.Audit - Error sending audit data to Audit Micro-service - Status: FAIL - Reason: {response.Content}");
                }
                catch (Exception e)
                {
                    _logService.LogErrorMessage($"AuditService.Audit - An error has occurred serializing entity with id = {entity.Id} for audit action = {action}");
                    _logService.LogErrorMessage($"AuditService.Audit - {e.Message}");
                }
            });

            task.Start();
        }
    }
}
