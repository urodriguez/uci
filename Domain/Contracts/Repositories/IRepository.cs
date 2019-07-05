using System.Collections.Generic;
using Domain.Contracts.Aggregates;

namespace Domain.Contracts.Repositories
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<TAggregateRoot> GetAll();
        TAggregateRoot GetById(int id);
        void Update(TAggregateRoot dto);
        void Remove(int id);
        void Add(TAggregateRoot aggregate);
    }
}
