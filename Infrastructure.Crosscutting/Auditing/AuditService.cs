using System.Collections.Generic;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.DequeueResolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using Newtonsoft.Json;

namespace Infrastructure.Crosscutting.Auditing
{
    public class AuditService : AsyncInfrastractureService, IAuditService, IAuditDequeueResolver
    {
        private readonly IAppSettingsService _appSettingsService;

        public AuditService(ILogService logService, IAppSettingsService appSettingsService, IEnqueueService queueService) : base(logService, queueService)
        {
            _appSettingsService = appSettingsService;

            UseBaseUrl(appSettingsService.AuditingApiUrlV1);
        }

        public void Audit(Audit audit)
        {
            audit.Credential = _appSettingsService.InfrastructureCredential;
            ExecuteAsync("audits", audit);
        }

        public void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData)
        {
            foreach (var queueItemJsonData in queueItemsJsonData)
            {
                var audit = JsonConvert.DeserializeObject<Audit>(queueItemJsonData);
                Audit(audit);
            }
        }

        public bool ResolvesQueueItemType(QueueItemType queueItemType) => queueItemType == QueueItemType.Audit;
    }
}
