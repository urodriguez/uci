namespace Domain.Contracts.Infrastructure.Crosscutting.Mailing
{
    public interface ISmtpServerConfiguration
    {
        ISender Sender { get; }
        IHost Host { get; }
    }
}