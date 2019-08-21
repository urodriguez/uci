using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
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
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                //return sqlConnection.GetList<TAggregateRoot>().ToList();
                var predicate = Predicates.Field<Product>(p => p.Price, Operator.Lt, maxPrice);
                var list = sqlConnection.GetList<Product>(predicate).ToList();

                return list;
            }
        }
    }
}
