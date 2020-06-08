using System;
using System.Threading.Tasks;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Domain.Contracts.Infrastructure.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IInventionRepository Inventions { get; }
        IUserRepository Users { get; }

        Task BeginTransactionAsync();
        void Commit();
        IRepository<TAggregateRoot> GetRepository<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;
    }
}