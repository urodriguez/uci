using Domain.Contracts.Repositories;
using Persistence.Repositories;

namespace Persistence
{
    public interface IUnitOfWork
    {
        IRepository Repository { get; }
        void SaveChanges();
    }
}