using System;

namespace Domain.Contracts.Aggregates
{
    public interface IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
