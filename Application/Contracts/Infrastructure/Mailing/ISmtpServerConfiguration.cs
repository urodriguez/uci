namespace Application.Contracts.Infrastructure.Mailing
{
    public interface ISmtpServerConfiguration
    {
        ISender Sender { get; set; }
        IHost Host { get; set; }
    }
}