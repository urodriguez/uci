using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public IEnumerable<Product> GetCheapest(decimal maxPrice)
        {
            //return Products.Where(p => p.Price < maxPrice);
            return new List<Product>();
        }
    }
}
