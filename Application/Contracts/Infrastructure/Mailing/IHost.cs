namespace Application.Contracts.Infrastructure.Mailing
{
    public interface IHost
    {
        string Name { get; set; }
        int Port { get; set; }
        bool UseSsl { get; set; }
    }
}