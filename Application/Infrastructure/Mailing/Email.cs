using System.Collections.Generic;
using Application.Contracts.Infrastructure.Mailing;
using Application.Infrastructure.Queueing;

namespace Application.Infrastructure.Mailing
{
    public class Email : IEmail
    {
        public Email()
        {
            UseCustomSmtpServer = false;
            QueueItemType = QueueItemType.Email;
        }

        public InfrastructureCredential Credential { get; set; }
        public bool UseCustomSmtpServer { get; set; }
        public ISmtpServerConfiguration SmtpServerConfiguration { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<IAttachment> Attachments { get; set; }
        public QueueItemType QueueItemType { get; set; }
    }
}