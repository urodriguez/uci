using System.Collections.Generic;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Infrastructure.Mailing;
using Application.Contracts.Infrastructure.Queueing.Enqueue;
using Application.Infrastructure.Mailing;
using Application.Infrastructure.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers;
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

        public void SendAsync(IEmail email)
        {
            email.Credential = _appSettingsService.InfrastructureCredential;
            ExecuteAsync("emails", Method.POST, email);
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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