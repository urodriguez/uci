using System;
using Domain.Contracts.Aggregates;

namespace Domain.Aggregates
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}