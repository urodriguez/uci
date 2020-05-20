using System;

namespace Domain.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            Name = $"{GetType().BaseType.Name}";
        }

        protected string Name { get; }

        public Guid Id { get; set; }
    }
}