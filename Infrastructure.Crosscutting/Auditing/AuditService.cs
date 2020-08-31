using System.Collections.Generic;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Auditing;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Infrastructure.Queueing.Enqueue;
using Application.Infrastructure.Auditing;
using Application.Infrastructure.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructure.Crosscutting.Auditing
{
    public class AuditService : InfrastractureService, IAuditService, IAuditDequeueResolver
    {
        private readonly IAppSettingsService _appSettingsService;

        public AuditService(ILogService logService, IAppSettingsService appSettingsService, IEnqueueService queueService) : base(logService, queueService)
        {
            _appSettingsService = appSettingsService;

            UseBaseUrl(appSettingsService.AuditingApiUrlV1);
        }

        public void AuditAsync(IAudit audit)
        {
            audit.Credential = _appSettingsService.InfrastructureCredential;
            ExecuteAsync("audits", Method.POST, audit);
        }

        public void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData)
        {
            foreach (var queueItemJsonData in queueItemsJsonData)
            {
                var audit = JsonConvert.DeserializeObject<Audit>(queueItemJsonData);
                AuditAsync(audit);
            }
        }

        public bool ResolvesQueueItemType(QueueItemType queueItemType) => queueItemType == QueueItemType.Audit;
    }
}
