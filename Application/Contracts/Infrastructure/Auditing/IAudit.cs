using Application.Contracts.Infrastructure.Queueing;
using Application.Infrastructure;
using Application.Infrastructure.Auditing;

namespace Application.Contracts.Infrastructure.Auditing
{
    public interface IAudit : IQueueable
    {
        InfrastructureCredential Credential { get; set; }
        string Application { get; set; }
        string Environment { get; set; }
        string User { get; set; }
        string EntityId { get; set; }
        string EntityName { get; set; }
        string Entity { get; set; }
        AuditAction Action { get; set; }
    }
}