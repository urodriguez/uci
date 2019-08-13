using System.Collections.Generic;
using Domain.Aggregates;

namespace Persistence
{
    public class UciDbContext : DbContext
    {
        public IList<Product> Products { get; set; }

        public UciDbContext()
        {
        }
    }
}