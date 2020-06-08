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
            return new InventAppPredicateIndividual<Invention>(p => p.Price, InventAppPredicateOperator.Le, maxPrice);
        }

        public IInventAppPredicate<Invention> CreateByDistinctIdAndCode(Guid id, string code)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Invention>(u => u.Code, InventAppPredicateOperator.Eq, code)
                },
                InventAppPredicateOperatorGroup.And
            );
        }

        public IInventAppPredicate<Invention> CreateByDistinctIdAndName(Guid id, string name)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Invention>(u => u.Name, InventAppPredicateOperator.Eq, name)
                },
                InventAppPredicateOperatorGroup.And
            );
        }

        public IInventAppPredicate<Invention> CreateByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Price, InventAppPredicateOperator.Ge, minPrice),
                    new InventAppPredicateIndividual<Invention>(u => u.Price, InventAppPredicateOperator.Le, maxPrice)
                },
                InventAppPredicateOperatorGroup.And
            );
        }
    }
}