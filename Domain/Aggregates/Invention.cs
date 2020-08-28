using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Aggregates
{
    public class Invention : Entity, IAggregateRoot
    {
        public Invention() {}

        public Invention(
            string code,
            string name,
            string category,
            decimal price
        )
        {
            SetCode(code);
            SetName(name);
            SetCategory(category);
            SetPrice(price);
        }

        public string Code { get; private set; }
        public void SetCode(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new BusinessRuleException($"{EntityName}: {PropertyName} can not be null or empty");
            if (code.Length != 8) throw new BusinessRuleException($"{EntityName}: {PropertyName} length must be 8");
            Code = code;
        }

        public string Name { get; private set; }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new BusinessRuleException($"{EntityName}: {PropertyName} can not be null or empty");
            if (name.Length >= 32) throw new BusinessRuleException($"{EntityName}: {PropertyName} length can not be greater than 32");
            Name = name;
        }

        public string Category { get; private set; }
        public void SetCategory(string category)
        {
            if (string.IsNullOrEmpty(category)) throw new BusinessRuleException($"{EntityName}: {PropertyName} can not be null or empty");
            Category = category;
        }

        public decimal Price { get; private set;  }
        public void SetPrice(decimal price)
        {
            if (price <= 0) throw new BusinessRuleException($"{EntityName}: {PropertyName} can not be less or equal than zero");
            Price = price;
        }
    }
}