namespace Domain.Contracts.Infrastructure.Crosscutting.Mailing
{
    public interface IEmailService
    {
        void Send(IEmail email);
    }
}