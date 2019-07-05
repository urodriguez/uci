using Domain.Contracts.Repositories;
using Persistence.Repositories;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UciDbContext _uciDbContext;
        public IRepository Repository { get; }

        public UnitOfWork(IRepository repository)
        {
            Repository = repository;
            _uciDbContext = new UciDbContext();
        }

        public void SaveChanges()
        {
            _uciDbContext.SaveChanges();
        }
    }
}