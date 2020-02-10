using Domain.Contracts.Infrastructure.Crosscutting.Mailing;

namespace Infrastructure.Crosscutting.Mailing
{
    public class Email : IEmail
    {
        public bool UseCustomSmtpServer { get; set; }
        public ISmtpServerConfiguration SmtpServerConfiguration { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}