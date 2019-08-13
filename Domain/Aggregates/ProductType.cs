using Domain.Contracts.Aggregates;
using Domain.Entities;

namespace Domain.Aggregates
{
    public class ProductType : Entity, IAggregateRoot
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}