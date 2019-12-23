using System;
using System.Linq.Expressions;
using Domain.Contracts.Aggregates;

namespace Domain
{
    public class FieldValue<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public Expression<Func<TAggregateRoot, object>> Field { get; set; }
        public object Value { get; set; }
    }
}