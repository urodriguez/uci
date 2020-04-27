using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Mailing
{
    public class Email 
    {
        public InfrastructureCredential Credential { get; set; }
        public bool UseCustomSmtpServer { get; set; }
        public SmtpServerConfiguration SmtpServerConfiguration { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}