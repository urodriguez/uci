using System;

namespace Infrastructure.Crosscutting.Queueing
{
    public class QueueItem
    {
        public QueueItem(QueueItemType type, string data)
        {
            Id = Guid.NewGuid();
            Type = type;
            Data = data;
            QueueDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public QueueItemType Type { get; set; }
        public string Data { get; set; }
        public DateTime QueueDate { get; set; }
    }
}