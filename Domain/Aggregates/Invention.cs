using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Enums;
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
            ValidateRequiredString(code, new[] { 8 });
            Code = code;
        }

        public string Name { get; private set; }
        public void SetName(string name)
        {
            ValidateRequiredString(name, new[] { 4, 32 });
            Name = name;
        }

        public string Category { get; private set; }
        public void SetCategory(string category)
        {
            ValidateRequiredString(category, new[] { 8 });
            Category = category;
        }

        public decimal Price { get; private set;  }
        public void SetPrice(decimal price)
        {
            ValidateRequiredDecimal(price, ComparisonOperator.Gt, 0);
            Price = price;
        }
    }
}