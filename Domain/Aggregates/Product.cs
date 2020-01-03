using System.Text.RegularExpressions;
using Domain.Attributes;
using Domain.Contracts.Aggregates;
using Domain.Entities;

namespace Domain.Aggregates
{
    public class Product : Entity, IAggregateRoot
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public decimal Price { get; set;  }
        //public Guid ProductTypeId { get; set; }

        public static bool PriceIsEqualOrHigherThanZero(decimal price)
        {
            return price >= 0;
        }
    }
}