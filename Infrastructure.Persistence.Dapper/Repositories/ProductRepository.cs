using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IDbConnectionFactory dbConnectionFactory, ILogService loggerService) : base(dbConnectionFactory, loggerService)
        {
        }

        public IEnumerable<Product> GetCheapest(decimal maxPrice)
        {
            //var pg = new InventAppPredicateGroup<Product>
            //{
            //    Operator = GroupOperator.And,
            //    Predicates = new List<IPredicate>
            //    {
            //        Predicates.Field<Product>(p => p.Price, Operator.Le, maxPrice),
            //        Predicates.Field<Product>(p => p.Category, Operator.Eq, "A")
            //    }
            //};
            return new List<Product>();//TODO
        }
    }
}
