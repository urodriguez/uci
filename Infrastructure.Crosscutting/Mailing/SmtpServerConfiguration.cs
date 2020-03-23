namespace Infrastructure.Crosscutting.Mailing
{
    public class SmtpServerConfiguration
    {
        public Sender Sender { get; set; }
        public Host Host { get; set; }
    }
}