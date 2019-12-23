using System.Collections.Generic;
using Domain.Contracts.Aggregates;
using Domain.Enums;

namespace Domain.Predicates
{
    public class InventAppPredicateGroup<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public IList<InventAppPredicate<TAggregateRoot>> Predicates { get; set; }
        public InventAppPredicateOperatorGroup OperatorGroup { get; set; }
    }
}