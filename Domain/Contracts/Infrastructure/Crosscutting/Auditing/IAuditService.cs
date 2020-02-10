namespace Domain.Contracts.Infrastructure.Crosscutting.Auditing
{
    public interface IAuditService
    {
        void Audit(IAuditable auditable);
    }
}