﻿using System;
using Application.Contracts.Infrastructure.Queueing;

namespace Application.Infrastructure.Queueing
{
    public class QueueItem : IQueueItem
    {
        //EF
        public QueueItem() { }

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