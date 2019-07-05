using System.Collections.Generic;
using Domain.Aggregates;

namespace Persistence
{
    public class UciDbContext : DbContext
    {
        public IList<Product> Products { get; set; }

        public UciDbContext()
        {
            Products = new List<Product>
            {
                new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
                new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
                new Product { Id = 3, Name = "Hammer1", Category = "Hardware", Price = 16.99M },
                new Product { Id = 4, Name = "Hammer2", Category = "Toys", Price = 16 },
                new Product { Id = 5, Name = "Hammer3", Category = "Groceries", Price = 20 },
                new Product { Id = 6, Name = "Hammer4", Category = "Hardware", Price = 21 }
            };
        }
    }
}