using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Logging;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILoggerService _loggerService;

        protected Repository(IDbConnectionFactory dbConnectionFactory, ILoggerService loggerService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _loggerService = loggerService;
            var x = typeof(Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                return sqlConnection.GetList<TAggregateRoot>().ToList();
            }
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                return sqlConnection.Get<TAggregateRoot>(id);
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Update(aggregateRoot);
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = Guid.NewGuid();
            
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Insert(aggregate);
            }
        }

        public void Delete(TAggregateRoot aggregate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Delete(aggregate);
            }
        }

        protected IEnumerable<TAggregateRoot> ExecuteGetList(IFieldPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResultList = sqlConnection.GetList<TAggregateRoot>(predicate).ToList();

                _loggerService.Log(new LogMessage(CustomDbProfiler.Current.GetCommands(), LogLevel.Trace));

                return dbResultList;
            }
        }

        protected TAggregateRoot ExecuteGet(IFieldPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.Get<TAggregateRoot>(predicate);

                _loggerService.Log(new LogMessage(CustomDbProfiler.Current.GetCommands(), LogLevel.Trace));

                return dbResult;
            }
        }
    }
}
