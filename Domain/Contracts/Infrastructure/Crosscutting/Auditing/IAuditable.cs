namespace Domain.Contracts.Infrastructure.Crosscutting.Auditing
{
    public interface IAuditable
    {
        string Application { get; set; }
        string Environment { get; set; }
        string User { get; set; }
        string Entity { get; set; }
        string EntityName { get; set; }
        AuditAction Action { get; set; }
    }
}