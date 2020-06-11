namespace Infrastructure.Crosscutting.Mailing
{
    public interface IEmailService
    {
        void SendAsync(Email email);
        bool EmailIsValid(string email);
    }
}