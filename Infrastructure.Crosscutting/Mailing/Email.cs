using System.Collections.Generic;
using Infrastructure.Crosscutting.Queueing;

namespace Infrastructure.Crosscutting.Mailing
{
    public class Email  : IQueueable
    {
        public Email()
        {
            UseCustomSmtpServer = false;
            QueueItemType = QueueItemType.Email;
        }

        public InfrastructureCredential Credential { get; set; }
        public bool UseCustomSmtpServer { get; set; }
        public SmtpServerConfiguration SmtpServerConfiguration { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public QueueItemType QueueItemType { get; set; }
    }
}