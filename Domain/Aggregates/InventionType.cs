using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Aggregates
{
    public class InventionType : Entity, IAggregateRoot
    {
        public InventionType() {}

        public InventionType(
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
            if (string.IsNullOrEmpty(code)) throw new BusinessRuleException($"{EntityName}: ${PropertyName} can not be null or empty");
            if (code.Length != 8) throw new BusinessRuleException($"{EntityName}: ${PropertyName} length must be 8");
            Code = code;
        }

        public string Name { get; private set; }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new BusinessRuleException($"{EntityName}: ${PropertyName} can not be null or empty");
            if (name.Length >= 32) throw new BusinessRuleException($"{EntityName}: ${PropertyName} length can not be greater than 32");
            Name = name;
        }

        public string Description { get; private set; }
        public void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description)) throw new BusinessRuleException($"{EntityName}: ${PropertyName} can not be null or empty");
            if (description.Length >= 128) throw new BusinessRuleException($"{EntityName}: ${PropertyName} length can not be greater than 128");
            Description = description;
        }
    }
}