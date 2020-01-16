namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface IEmailService
    {
        void Send(string to, string from, string subject, string body);
    }
}