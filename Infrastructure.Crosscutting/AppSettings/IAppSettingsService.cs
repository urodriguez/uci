namespace Infrastructure.Crosscutting.AppSettings
{
    public interface IAppSettingsService
    {
        string AuditingApiUrlV1 { get; }
        string AuthenticationApiUrlV1 { get; }
        string ClientUrl { get; }
        string ConnectionString { get; }
        int DefaultTokenExpiresTime { get; }
        InventAppEnvironment Environment { get; }
        string FileSystemLogsDirectory { get; }
        string HangfireInventAppConnectionString { get; }
        string InventAppDirectory { get; }
        InfrastructureCredential InfrastructureCredential { get; }
        string LoggingApiUrlV1 { get; }
        string MailingApiUrlV1 { get; }
        string WebApiUrl { get; }
    }
}