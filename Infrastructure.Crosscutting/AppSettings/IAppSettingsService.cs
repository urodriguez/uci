namespace Infrastructure.Crosscutting.AppSettings
{
    public interface IAppSettingsService
    {
        string AuditingApiUrl { get; }
        string AuthenticationApiUrl { get; }
        string ClientUrl { get; }
        string ConnectionString { get; }
        int DefaultTokenExpiresTime { get; }
        InventAppEnvironment Environment { get; }
        InfrastructureCredential InfrastructureCredential { get; }
        string LoggingApiUrl { get; }
        string MailingApiUrl { get; }
        string WebApiUrl { get; }
    }
}