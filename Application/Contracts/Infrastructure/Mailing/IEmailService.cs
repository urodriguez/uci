namespace Application.Contracts.Infrastructure.Mailing
{
    public interface IEmailService
    {
        void SendAsync(IEmail email);
        bool EmailIsValid(string email);
    }
}