using System;
using Domain.Aggregates;

namespace Domain.Contracts.Predicates.Factories
{
    public interface IProductPredicateFactory : IPredicateFactory<Product>
    {
        IInventAppPredicate<Product> CreateByCheapest(decimal maxPrice);
        IInventAppPredicate<Product> CreateByDistinctIdAndCode(Guid id, string code);
        IInventAppPredicate<Product> CreateByDistinctIdAndName(Guid id, string name);
        IInventAppPredicate<Product> CreateByPriceRange(decimal minPrice, decimal maxPrice);
    }
}