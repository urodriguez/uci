using System;
using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Contracts.Predicates;
using Domain.Contracts.Predicates.Factories;
using Domain.Enums;

namespace Domain.Predicates.Factories
{
    public class ProductPredicateFactory : IProductPredicateFactory
    {
        public IInventAppPredicate<Product> CreateByCheapest(decimal maxPrice)
        {
            return new InventAppPredicateIndividual<Product>(p => p.Price, InventAppPredicateOperator.Le, maxPrice);
        }

        public IInventAppPredicate<Product> CreateByDistinctIdAndCode(Guid id, string code)
        {
            return new InventAppPredicateGroup<Product>(
                new List<IInventAppPredicate<Product>>
                {
                    new InventAppPredicateIndividual<Product>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Product>(u => u.Code, InventAppPredicateOperator.Eq, code)
                },
                InventAppPredicateOperatorGroup.And
            );
        }

        public IInventAppPredicate<Product> CreateByDistinctIdAndName(Guid id, string name)
        {
            return new InventAppPredicateGroup<Product>(
                new List<IInventAppPredicate<Product>>
                {
                    new InventAppPredicateIndividual<Product>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Product>(u => u.Name, InventAppPredicateOperator.Eq, name)
                },
                InventAppPredicateOperatorGroup.And
            );
        }
    }
}