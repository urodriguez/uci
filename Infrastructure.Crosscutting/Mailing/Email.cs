namespace Infrastructure.Crosscutting.Mailing
{
    public class Email 
    {
        public InfrastructureAccount Account { get; set; }
        public bool UseCustomSmtpServer { get; set; }
        public SmtpServerConfiguration SmtpServerConfiguration { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}