using System.Collections.Generic;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
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

        public void AuditAsync(Audit audit)
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
