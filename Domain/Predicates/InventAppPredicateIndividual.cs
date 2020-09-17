using System;
using System.Linq.Expressions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Predicates;
using Domain.Enums;

namespace Domain.Predicates
{
    public class InventAppPredicateIndividual<TAggregateRoot> : IInventAppPredicate<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public InventAppPredicateIndividual(Expression<Func<TAggregateRoot, object>> field, ComparisonOperator @operator, object value)
        {
            Field = field;
            Operator = @operator;
            Value = value;
        }

        public Expression<Func<TAggregateRoot, object>> Field { get; }
        public ComparisonOperator Operator { get; }
        public object Value { get; }

        public void Add(IInventAppPredicate<TAggregateRoot> inventAppPredicate)
        {
            throw new NotSupportedException(GetType().Name + " can not 'Add' child elements");
        }
    }
}