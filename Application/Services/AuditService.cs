using System;
using System.Threading;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Aggregates;
using Domain.Enums;
using Infrastructure.Crosscutting.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogService _logService;

        public AuditService(ILogService logService)
        {
            _logService = logService;
        }

        //TODO: implement async + avoid lost audit if connection fails
        public void Audit(IAggregateRoot entity, AuditAction action, IAggregateRoot oldEntity = null)
        {
            try
            {
                _logService.QueueInfoMessage($"AuditService.Audit - Serializing entity with id = {entity.Id} for audit action = {action}");

                var entityJson = JsonConvert.SerializeObject(entity);
                var oldEntityJson = oldEntity != null ? JsonConvert.SerializeObject(oldEntity) : new JObject().ToString();

                //sent data to WebApi.Audit
                var client = new RestClient("https://localhost:44366/api");

                var audit = new AuditDto
                {
                    Action = action,
                    ApplicationCode = 0,
                    EntityId = entity.Id,
                    Entity = entityJson,
                    OldEntity = oldEntityJson,
                    User = Thread.CurrentPrincipal.Identity.Name
                };

                var request = new RestRequest("audits", Method.POST);
                request.AddJsonBody(audit);

                _logService.QueueInfoMessage("AuditService.Audit - Send audit data to Audit Micro-service: START");
                var response = client.Post(request);
                _logService.QueueInfoMessage(response.IsSuccessful ? "AuditService.Audit - Send audit data to Audit Micro-service: END - Status: OK" : $"AuditService.Audit - Send audit data to Audit Micro-service: END - Status: FAIL - Reason: {response.Content}");
            }
            catch (Exception e)
            {
                _logService.QueueErrorMessage($"AuditService.Audit - An error has occurred serializing entity with id = {entity.Id} for audit action = {action}");
                _logService.QueueErrorMessage($"AuditService.Audit - {e.Message}");
            }
        }
    }
}