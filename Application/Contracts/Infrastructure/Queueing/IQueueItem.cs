using System;
using Application.Infrastructure.Queueing;

namespace Application.Contracts.Infrastructure.Queueing
{
    public interface IQueueItem
    {
        Guid Id { get; set; }
        QueueItemType Type { get; set; }
        string Data { get; set; }
        DateTime QueueDate { get; set; }
    }
}