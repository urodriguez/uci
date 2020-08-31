using Application.Infrastructure.Queueing;

namespace Application.Contracts.Infrastructure.Queueing
{
    public interface IQueueable
    {
        QueueItemType QueueItemType { get; set; }
    }
}