using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Queueing.Dequeue.DequeueResolvers
{
    public interface IDequeueResolver
    {
        void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData);
        bool ResolvesQueueItemType(QueueItemType queueItemType);
    }
}