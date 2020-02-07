namespace Domain.Contracts.Infrastructure.Crosscutting.Mailing
{
    public interface IHost
    {
        string Name { get; }
        int Port { get; }
        bool UseSsl { get; }
    }
}