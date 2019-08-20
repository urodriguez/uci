using System;
using System.Collections.Generic;
using Domain.Contracts.Aggregates;

namespace Domain.Contracts.Repositories
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<TAggregateRoot> GetAll();
        TAggregateRoot GetById(Guid id);
        void Update(TAggregateRoot dto);
        void Delete(Guid id);
        void Add(TAggregateRoot aggregate);
    }
}
