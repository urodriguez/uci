using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Persistence.Dapper.Repositories;

namespace Infrastructure.Persistence.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogService _logService;
        private readonly IAppSettingsService _appSettingsService;

        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        private IDictionary<string, object> _aggregatesRepositories;

        public UnitOfWork(ILogService logService, IAppSettingsService appSettingsService, IDbConnectionFactory dbConnectionFactory)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;

            _connection = dbConnectionFactory.GetSqlConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            InitializeRepositories();
        }

        public IProductRepository Products { get; private set; }
        public IUserRepository Users { get; private set; }

        private void InitializeRepositories()
        {
            Products = new ProductRepository(_logService, _appSettingsService, _transaction);
            Users = new UserRepository(_logService, _appSettingsService, _transaction);

            _aggregatesRepositories = new Dictionary<string, object>();
            foreach (PropertyInfo property in typeof(UnitOfWork).GetProperties())
            {
                var aggregateName = property.Name.Remove(property.Name.Length - 1);//removes the final 's'
                _aggregatesRepositories.Add(aggregateName, property.GetValue(this));
            }
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