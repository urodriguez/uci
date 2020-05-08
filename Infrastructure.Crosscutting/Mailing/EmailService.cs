using System.Collections.Generic;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.DequeueResolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using Newtonsoft.Json;

namespace Infrastructure.Crosscutting.Mailing
{
    public class EmailService : AsyncInfrastractureService, IEmailService, IEmailDequeueResolver
    {
        private readonly IAppSettingsService _appSettingsService;

        public EmailService(ILogService logService, IAppSettingsService appSettingsService, IEnqueueService queueService) : base(logService, queueService)
        {
            _appSettingsService = appSettingsService;

            UseBaseUrl(appSettingsService.MailingApiUrlV1);
        }

        public void Send(Email email)
        {
            email.Credential = _appSettingsService.InfrastructureCredential;
            ExecuteAsync("emails", email);
        }

        public void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData)
        {
            foreach (var queueItemJsonData in queueItemsJsonData)
            {
                var email = JsonConvert.DeserializeObject<Email>(queueItemJsonData);
                Send(email);
            }
        }

        public bool ResolvesQueueItemType(QueueItemType queueItemType) => queueItemType == QueueItemType.Email;
    }
}