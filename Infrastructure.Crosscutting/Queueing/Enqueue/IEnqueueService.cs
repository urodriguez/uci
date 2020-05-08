namespace Infrastructure.Crosscutting.Queueing.Enqueue
{
    public interface IEnqueueService
    {
        void Execute(IQueueable queueable, QueueItemType type);
    }
}