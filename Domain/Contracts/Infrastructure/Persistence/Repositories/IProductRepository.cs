using System.Collections.Generic;
using Domain.Aggregates;

namespace Domain.Contracts.Infrastructure.Persistence.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetCheapest(decimal maxPrice);
    }
}
