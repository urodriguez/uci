using Application.Infrastructure.Queueing;

namespace Application.Contracts.Infrastructure.Queueing.Enqueue
{
    public interface IEnqueueService
    {
        void Execute(IQueueable queueable, QueueItemType type);
    }
}