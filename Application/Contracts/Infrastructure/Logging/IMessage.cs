using Application.Contracts.Infrastructure.Queueing;
using Application.Infrastructure;
using Application.Infrastructure.Logging;

namespace Application.Contracts.Infrastructure.Logging
{
    public interface IMessage :  IQueueable
    {
        InfrastructureCredential Credential { get; set; }
        string Application { get; set; }
        string Project { get; set; }
        string CorrelationId { get; set; }
        string Text { get; set; }
        MessageType Type { get; set; }
        string Environment { get; set; }
    }
}