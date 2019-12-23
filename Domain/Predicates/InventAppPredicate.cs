using System;
using System.Linq.Expressions;
using Domain.Contracts.Aggregates;
using Domain.Enums;

namespace Domain.Predicates
{
    public class InventAppPredicate<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public Expression<Func<TAggregateRoot, object>> Field { get; set; }
        public InventAppPredicateOperator Operator { get; set; }
        public object Value { get; set; }
    }
}