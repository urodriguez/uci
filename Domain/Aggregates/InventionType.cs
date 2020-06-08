using Domain.Contracts.Aggregates;
using Domain.Entities;

namespace Domain.Aggregates
{
    public class InventionType : Entity, IAggregateRoot
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}