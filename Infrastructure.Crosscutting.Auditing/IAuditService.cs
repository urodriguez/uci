using Domain.Contracts.Aggregates;

namespace Infrastructure.Crosscutting.Auditing
{
    public interface IAuditService
    {
        void Audit(IAggregateRoot entity, AuditAction action, IAggregateRoot oldEntity = null);
    }
}