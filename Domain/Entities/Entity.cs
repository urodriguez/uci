using System;

namespace Domain.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            EntityName = $"{GetType().BaseType.Name}";
        }

        protected string EntityName { get; }

        public Guid Id { get; set; }
    }
}