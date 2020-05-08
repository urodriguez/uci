namespace Infrastructure.Crosscutting.Queueing
{
    public interface IQueueable
    {
        QueueItemType QueueItemType { get; set; }
    }
}