using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
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
        private readonly IDbConnectionFactory _dbConnectionFactory;

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IDictionary<string, object> _aggregatesRepositories;

        public UnitOfWork(ILogService logService, IAppSettingsService appSettingsService, IDbConnectionFactory dbConnectionFactory)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IInventionRepository Inventions { get; private set; }
        public IUserRepository Users { get; private set; }

        public async Task BeginTransactionAsync()
        {
            _connection = await _dbConnectionFactory.GetOpenedSqlConnectionAsync();

            _transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            InitializeRepositoriesWithTransaction(_transaction);
        }

        private void InitializeRepositoriesWithTransaction(IDbTransaction transaction)
        {
            Inventions = new InventionRepository(_logService, _appSettingsService, transaction);
            Users = new UserRepository(_logService, _appSettingsService, transaction);

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
                //Commits the database transaction.
                //makes all data modifications since the start of the transaction a permanent part of the database, frees the transaction's resources
                _transaction.Commit();
            }
            catch
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Transaction Commit: an Exception has occurred. See more details at LogError level");

                try
                {
                    //Attempt to roll back the transaction.
                    //undo all data modifications since the start of the transaction deleting changes made in database, frees the transaction's resources
                    _transaction.Rollback();
                }
                catch
                {
                    //This catch block will handle any errors that may have occurred on the server that would cause the rollback to fail, 
                    //such as a closed connection.
                    _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Transaction Rollback: an Exception has occurred. See more details at LogError level");
                    throw;
                }
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
                _transaction.Dispose();//any still running transactions will be rolled back if no Commit was applied

            if (_connection != null)
            {
                //The Close method also will roll back any pending transactions if it wasn't commited
                //It then releases the connection to the connection pool, or closes the connection if connection pooling is disabled
                _connection.Close();
                _connection.Dispose();
            }

            //Dispose() usually calls GC.SuppressFinalize(this), meaning that Garbage Collector doesn't run the finalizer
            //It simply throws the memory away (much cheaper)
            GC.SuppressFinalize(this);
        }
    }
}