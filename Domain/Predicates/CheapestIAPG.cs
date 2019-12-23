using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Enums;

namespace Domain.Predicates
{
    public class CheapestIAPG : InventAppPredicateGroup<Product>
    {
        public CheapestIAPG(decimal maxPrice)
        {
            OperatorGroup = InventAppPredicateOperatorGroup.And;
            Predicates = new List<InventAppPredicate<Product>>
            {
                new InventAppPredicate<Product>
                {
                    Field = p => p.Price,
                    Operator = InventAppPredicateOperator.Le,
                    Value = maxPrice
                },                new InventAppPredicate<Product>
                {
                    Field = p => p.Category,
                    Operator = InventAppPredicateOperator.Eq,
                    Value = "A"
                }
            };
        }
    }
}