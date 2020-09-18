using System;
using Domain.Aggregates;

namespace Domain.Contracts.Predicates.Factories
{
    public interface IInventionCategoryPredicateFactory : IPredicateFactory<Invention>
    {
        IInventAppPredicate<Invention> CreateByDistinctIdAndCode(Guid id, string code);
        IInventAppPredicate<Invention> CreateByDistinctIdAndName(Guid id, string name);
    }
}