using System;
using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Aggregates
{
    public class Invention : Entity, IAggregateRoot
    {
        public Invention() {}

        public Invention(
            Guid userId,
            string code,
            string name,
            string description,
            Guid categoryId,
            decimal price,
            bool enable
        )
        {
            UserId = userId;
            SetCode(code);
            SetName(name);
            Description = description;
            CategoryId = categoryId;
            SetPrice(price);
            Enable = enable;
        }

        public Guid UserId { get; set; }

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

        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public decimal Price { get; private set;  }
        public void SetPrice(decimal price)
        {
            ValidateRequiredDecimal(price, ComparisonOperator.Gt, 0);
            Price = price;
        }

        public bool Enable { get; set; }
    }
}