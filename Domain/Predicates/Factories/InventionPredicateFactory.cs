using System;
using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Contracts.Predicates;
using Domain.Contracts.Predicates.Factories;
using Domain.Enums;

namespace Domain.Predicates.Factories
{
    public class InventionPredicateFactory : IInventionPredicateFactory
    {
        public IInventAppPredicate<Invention> CreateByCheapest(decimal maxPrice)
        {
            return new InventAppPredicateIndividual<Invention>(p => p.Price, ComparisonOperator.Le, maxPrice);
        }

        public IInventAppPredicate<Invention> CreateByDistinctIdAndCode(Guid id, string code)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Id, ComparisonOperator.NotEq, id),
                    new InventAppPredicateIndividual<Invention>(u => u.Code, ComparisonOperator.Eq, code)
                },
                ComparisonOperatorGroup.And
            );
        }

        public IInventAppPredicate<Invention> CreateByDistinctIdAndName(Guid id, string name)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Id, ComparisonOperator.NotEq, id),
                    new InventAppPredicateIndividual<Invention>(u => u.Name, ComparisonOperator.Eq, name)
                },
                ComparisonOperatorGroup.And
            );
        }

        public IInventAppPredicate<Invention> CreateByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Price, ComparisonOperator.Ge, minPrice),
                    new InventAppPredicateIndividual<Invention>(u => u.Price, ComparisonOperator.Le, maxPrice)
                },
                ComparisonOperatorGroup.And
            );
        }
    }
}