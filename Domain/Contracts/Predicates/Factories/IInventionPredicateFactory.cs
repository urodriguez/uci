using System;
using Domain.Aggregates;

namespace Domain.Contracts.Predicates.Factories
{
    public interface IInventionPredicateFactory : IPredicateFactory<Invention>
    {
        IInventAppPredicate<Invention> CreateByCheapest(decimal maxPrice);
        IInventAppPredicate<Invention> CreateByDistinctIdAndCode(Guid id, string code);
        IInventAppPredicate<Invention> CreateByDistinctIdAndName(Guid id, string name);
        IInventAppPredicate<Invention> CreateByPriceRange(decimal minPrice, decimal maxPrice);
    }
}