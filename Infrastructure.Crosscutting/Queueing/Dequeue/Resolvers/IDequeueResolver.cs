using System.Collections.Generic;
using Application.Infrastructure.Queueing;

namespace Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers
{
    public interface IDequeueResolver
    {
        void ResolveDequeue(IReadOnlyCollection<string> queueItemsJsonData);
        bool ResolvesQueueItemType(QueueItemType queueItemType);
    }
}