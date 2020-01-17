using Domain.Contracts.Aggregates;
using Domain.Enums;

namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface IAuditService
    {
        void Audit(IAggregateRoot entity, AuditAction action);
    }
}