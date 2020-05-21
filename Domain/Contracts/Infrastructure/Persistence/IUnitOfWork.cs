using System;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Domain.Contracts.Infrastructure.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IUserRepository Users { get; }

        void Commit();
        IRepository<TAggregateRoot> GetRepository<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;
    }
}