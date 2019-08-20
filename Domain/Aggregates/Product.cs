using Domain.Contracts.Aggregates;
using Domain.Entities;

namespace Domain.Aggregates
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        //public Guid ProductTypeId { get; set; }
    }
}