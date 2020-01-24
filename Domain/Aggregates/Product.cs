using Domain.Attributes;
using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Aggregates
{
    public class Product : Entity, IAggregateRoot
    {
        [Required]
        public string Code { get; private set; }

        [Required]
        public string Name { get; private set; }

        [Required]
        public string Category { get; private set; }

        [Required]
        public decimal Price { get; private set;  }
        //public Guid ProductTypeId { get; set; }

        public void SetCode(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new BusinessRuleException($"{EntityName}: code can not be null or empty");
            Code = code;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new BusinessRuleException($"{EntityName}: name can not be null or empty");
            Name = name;
        }

        public void SetCategory(string category)
        {
            if (string.IsNullOrEmpty(category)) throw new BusinessRuleException($"{EntityName}: category can not be null or empty");
            Category = category;
        }

        public void SetPrice(decimal price)
        {
            if (price <= 0) throw new BusinessRuleException($"{EntityName}: price has to be equal or higher than zero");
            Price = price;
        }

        public static void ValidatePrice(decimal price)
        {
            if (price <= 0) throw new BusinessRuleException($"{typeof(Product).Name}: price has to be equal or higher than zero");
        }
    }
}