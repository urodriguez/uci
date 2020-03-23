namespace Infrastructure.Crosscutting.Mailing
{
    public interface IEmailService
    {
        void Send(Email email);
    }
}