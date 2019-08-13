using System.Collections.Generic;
using Domain.Aggregates;

namespace Domain.Contracts.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetCheapest(decimal maxPrice);
    }
}
