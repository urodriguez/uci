using System.Collections.Generic;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.DequeueResolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructure.Crosscutting.Mailing
{
    public class EmailService : InfrastractureService, IEmailService, IEmailDequeueResolver
    {
        private readonly IAppSettingsService _appSettingsService;

        public EmailService(ILogService logService, IAppSettingsService appSettingsService, IEnqueueService queueService) : base(logService, queueService)
        {
            _appSettingsService = appSettingsService;

            UseBaseUrl(appSettingsService.MailingApiUrlV1);
        }

        public void SendAsync(Email email)
        {
            email.Credential = _appSettingsService.InfrastructureCredential;
            ExecuteAsync("emails", Method.POST, email);
        }

        public void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData)
        {
            foreach (var queueItemJsonData in queueItemsJsonData)
            {
                var email = JsonConvert.DeserializeObject<Email>(queueItemJsonData);
                SendAsync(email);
            }
        }

        public bool ResolvesQueueItemType(QueueItemType queueItemType) => queueItemType == QueueItemType.Email;
    }
}