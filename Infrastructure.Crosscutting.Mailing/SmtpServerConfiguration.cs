using Domain.Contracts.Infrastructure.Crosscutting.Mailing;

namespace Infrastructure.Crosscutting.Mailing
{
    public class SmtpServerConfiguration : ISmtpServerConfiguration
    {
        public ISender Sender { get; set; }
        public IHost Host { get; set; }
    }
}