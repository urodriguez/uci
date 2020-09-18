using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Aggregates
{
    public class InventionCategory : Entity, IAggregateRoot
    {
        public InventionCategory() {}

        public InventionCategory(
            string code,
            string name,
            string description
        )
        {
            SetCode(code);
            SetName(name);
            SetDescription(description);
        }

        public string Code { get; private set; }
        public void SetCode(string code)
        {
            ValidateRequiredString(code, new[] { 8 });
            Code = code;
        }

        public string Name { get; private set; }
        public void SetName(string name)
        {
            ValidateRequiredString(name, new[] { 4, 32 });
            Name = name;
        }

        public string Description { get; private set; }
        public void SetDescription(string description)
        {
            ValidateRequiredString(description, new[] { 4, 128 });
            Description = description;
        }
    }
}