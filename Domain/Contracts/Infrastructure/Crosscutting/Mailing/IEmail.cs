namespace Domain.Contracts.Infrastructure.Crosscutting.Mailing
{
    public interface IEmail
    {
        bool UseCustomSmtpServer { get; }
        ISmtpServerConfiguration SmtpServerConfiguration { get; }
        string To { get; }
        string Subject { get; }
        string Body { get; }
    }
}