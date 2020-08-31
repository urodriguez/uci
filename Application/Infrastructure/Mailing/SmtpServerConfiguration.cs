using Application.Contracts.Infrastructure.Mailing;

namespace Application.Infrastructure.Mailing
{
    public class SmtpServerConfiguration : ISmtpServerConfiguration
    {
        public ISender Sender { get; set; }
        public IHost Host { get; set; }
    }
}