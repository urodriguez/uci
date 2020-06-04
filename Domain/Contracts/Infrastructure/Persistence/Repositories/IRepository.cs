using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Predicates;

namespace Domain.Contracts.Infrastructure.Persistence.Repositories
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        Task<IEnumerable<TAggregateRoot>> GetAsync(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null);
        
        Task<TAggregateRoot> GetFirstAsync(IInventAppPredicate<TAggregateRoot> inventAppPredicate);
        
        Task<bool> AnyAsync(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null);

        Task<TAggregateRoot> GetByIdAsync(Guid id);

        Task UpdateAsync(TAggregateRoot dto);

        Task DeleteAsync(TAggregateRoot aggregate);

        Task AddAsync(TAggregateRoot aggregate);

        Task<bool> ContainsAsync(TAggregateRoot aggregate);
    }
}
