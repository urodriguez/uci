namespace Infrastructure.Crosscutting.AppSettings
{
    public interface IAppSettingsService
    {
        string AuditingUrl { get; }
        string AuthenticationUrl { get; }
        string ClientUrl { get; }
        string ConnectionString { get; }
        int DefaultTokenExpiresTime { get; }
        InventAppEnvironment Environment { get; }
        InfrastructureAccount InfrastructureAccount { get; }
        string LoggingUrl { get; }
        string MailingUrl { get; }
        string WebApiUrl { get; }
    }
}