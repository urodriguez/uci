using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;

namespace Infrastructure.Crosscutting.Mailing
{
    public class EmailService : AsyncInfrastractureService, IEmailService
    {
        private readonly IAppSettingsService _appSettingsService;

        public EmailService(ILogService logService, IAppSettingsService appSettingsService, IQueueService queueService) : base(logService, queueService)
        {
            _appSettingsService = appSettingsService;

            UseBaseUrl(appSettingsService.MailingApiUrlV1);
        }

        public void Send(Email email)
        {
            email.Credential = _appSettingsService.InfrastructureCredential;

            ExecuteAsync("emails", email);
        }
    }
}