namespace Infrastructure.Crosscutting.AppSettings
{
    public interface IAppSettingsService
    {
        string AssetsDirectory { get; }
        string AuditingApiUrlV1 { get; }
        string AuthenticationApiUrlV1 { get; }
        string BaseInventAppApiUrl { get; set; }
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
        string ReportingApiUrlV1 { get; }
        string ReportsDirectory { get; }
        string TemplatesDirectory { get; }
        string WebApiUrl { get; }
    }
}