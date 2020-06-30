using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers
{
    public interface IDequeueResolver
    {
        void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData);
        bool ResolvesQueueItemType(QueueItemType queueItemType);
    }
}