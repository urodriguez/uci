using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;

namespace Infrastructure.Crosscutting.Auditing
{
    public class AuditService : AsyncInfrastractureService, IAuditService
    {
        private readonly IAppSettingsService _appSettingsService;

        public AuditService(ILogService logService, IAppSettingsService appSettingsService, IQueueService queueService) : base(logService, queueService)
        {
            _appSettingsService = appSettingsService;

            UseBaseUrl(appSettingsService.AuditingApiUrlV1);
        }

        public void Audit(Audit audit)
        {
            audit.Credential = _appSettingsService.InfrastructureCredential;

            ExecuteAsync("audits", audit);
        }
    }
}
