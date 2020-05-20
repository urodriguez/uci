using System;
using System.Collections.Generic;
using System.Data;
using Domain.Aggregates;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Persistence.Dapper.Repositories;

namespace Infrastructure.Persistence.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogService _logService;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IDictionary<string, object> _aggregatesRepositories;

        public UnitOfWork(ILogService logService, IDbConnectionFactory dbConnectionFactory)
        {
            _logService = logService;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IProductRepository Products { get; set; }
        public IUserRepository Users { get; set; }

        public void BeginTransaction()
        {
            _connection = _dbConnectionFactory.GetSqlConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            Products = new ProductRepository(_logService, _transaction);
            Users = new UserRepository(_logService, _transaction);

            _aggregatesRepositories = new Dictionary<string, object>
            {
                {typeof(Product).Name, Products}, 
                {typeof(User).Name, Users}
            };
            
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }

        public IRepository<TAggregateRoot> GetRepository<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
        {
            return (IRepository<TAggregateRoot>)_aggregatesRepositories[typeof(TAggregateRoot).Name];
        }        

        //https://stackoverflow.com/questions/339063/what-is-the-difference-between-using-idisposable-vs-a-destructor-in-c
        //Dispose is used to deterministically clean up objects
        //It doesn't collect the object's memory(that still belongs to GC)
        //But is used for example to close files, database connections, etc.
        //It is not uncommon for an IDisposable object to also have a finalizer
        public void Dispose()
        {
            if (_transaction != null) 
                _transaction.Dispose();

            if (_connection != null)
            {
                //The Close method rolls back any pending transactions.
                //It then releases the connection to the connection pool, or closes the connection if connection pooling is disabled.
                _connection.Close();
                _connection.Dispose();
            }

            //Dispose() usually calls GC.SuppressFinalize(this), meaning that Garbage Collector doesn't run the finalizer
            //It simply throws the memory away (much cheaper)
            GC.SuppressFinalize(this);
        }
    }
}