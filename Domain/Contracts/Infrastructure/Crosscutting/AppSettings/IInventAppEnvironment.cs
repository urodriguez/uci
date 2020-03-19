namespace Domain.Contracts.Infrastructure.Crosscutting.AppSettings
{
    public interface IInventAppEnvironment
    {
        string Name { get; set; }
        bool IsLocal();
    }
}