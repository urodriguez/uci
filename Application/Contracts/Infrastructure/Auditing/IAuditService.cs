namespace Application.Contracts.Infrastructure.Auditing
{
    public interface IAuditService
    {
        void AuditAsync(IAudit audit);
    }
}