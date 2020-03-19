namespace Domain.Contracts.Infrastructure.Crosscutting.AppSettings
{
    public interface IAppSettingsService
    {
        string AuditingUrl { get; }
        string AuthenticationUrl { get; }
        string ClientUrl { get; }
        string ConnectionString { get; }
        IInventAppEnvironment Environment { get; }
        string LoggingUrl { get; }
        string MailingUrl { get; }
        string WebApiUrl { get; }
    }
}