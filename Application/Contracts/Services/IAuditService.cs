using System.Threading.Tasks;
using Domain.Contracts.Aggregates;
using Domain.Enums;

namespace Application.Contracts.Services
{
    public interface IAuditService
    {
        void Audit(IAggregateRoot entity, AuditAction action, IAggregateRoot oldEntity = null);
    }
}