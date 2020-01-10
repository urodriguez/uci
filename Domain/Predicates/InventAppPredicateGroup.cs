using System.Collections.Generic;
using Domain.Contracts.Aggregates;
using Domain.Enums;

namespace Domain.Predicates
{
    public class InventAppPredicateGroup<TAggregateRoot> : IInventAppPredicate<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public IList<IInventAppPredicate<TAggregateRoot>> Predicates { get; set; }
        public InventAppPredicateOperatorGroup Operator { get; set; }
    }
}