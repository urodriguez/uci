using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Contracts.Aggregates;
using Domain.Predicates;

namespace Domain.Contracts.Repositories
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<TAggregateRoot> Get(InventAppPredicate<TAggregateRoot> inventAppPredicate = null);

        IEnumerable<TAggregateRoot> Get(InventAppPredicateGroup<TAggregateRoot> inventAppPredicateGroup);

        IEnumerable<TAggregateRoot> GetByFields(IEnumerable<FieldValue<TAggregateRoot>> fieldValues);

        IEnumerable<TAggregateRoot> GetByField(Expression<Func<TAggregateRoot, object>> field, object value);

        TAggregateRoot GetById(Guid id);

        void Update(TAggregateRoot dto);

        void Delete(TAggregateRoot aggregate);

        void Add(TAggregateRoot aggregate);

        bool Contains(TAggregateRoot aggregate);
    }
}
