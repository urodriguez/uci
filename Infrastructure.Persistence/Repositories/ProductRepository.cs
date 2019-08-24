using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IDbConnectionFactory dbConnectionFactory, ILoggerService loggerService) : base(dbConnectionFactory, loggerService)
        {
        }

        public IEnumerable<Product> GetCheapest(decimal maxPrice)
        {
            return ExecuteGetList(Predicates.Field<Product>(p => p.Price, Operator.Le, maxPrice)).ToList();
        }
    }
}
