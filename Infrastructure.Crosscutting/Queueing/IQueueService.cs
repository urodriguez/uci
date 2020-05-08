namespace Infrastructure.Crosscutting.Queueing
{
    public interface IQueueService
    {
        void Enqueue(QueueItemType type, IQueueable queueable);
    }
}