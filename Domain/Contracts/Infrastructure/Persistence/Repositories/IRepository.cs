using System;
using System.Collections.Generic;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Predicates;

namespace Domain.Contracts.Infrastructure.Persistence.Repositories
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<TAggregateRoot> Get(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null);

        TAggregateRoot GetById(Guid id);

        void Update(TAggregateRoot dto);

        void Delete(TAggregateRoot aggregate);

        void Add(TAggregateRoot aggregate);

        bool Contains(TAggregateRoot aggregate);
    }
}
