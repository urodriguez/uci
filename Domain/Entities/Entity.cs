using System;

namespace Domain.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            EntityName = $"{GetType().BaseType.Name}";
        }

        public Guid Id { get; set; }
        protected string EntityName { get; }
    }
}