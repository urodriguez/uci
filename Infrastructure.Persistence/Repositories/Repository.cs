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
        private readonly QueryFormatter _queryFormatter;

        protected Repository(IDbConnectionFactory dbConnectionFactory, ILoggerService loggerService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _loggerService = loggerService;
            _queryFormatter = new QueryFormatter();

            var x = typeof(Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregates = sqlConnection.GetList<TAggregateRoot>().ToList();

                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));

                return aggregates;
            }
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregate = sqlConnection.Get<TAggregateRoot>(id);

                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));

                return aggregate;
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Update(aggregateRoot);
                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = Guid.NewGuid();
            
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Insert(aggregate);
                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));
            }
        }

        public void Delete(TAggregateRoot aggregate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Delete(aggregate);
                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));
            }
        }

        protected IEnumerable<TAggregateRoot> ExecuteGetList(IPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResultList = sqlConnection.GetList<TAggregateRoot>(predicate).ToList();

                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));

                return dbResultList;
            }
        }

        protected TAggregateRoot ExecuteGet(IPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.Get<TAggregateRoot>(predicate);

                _loggerService.QueueMessageTrace(_queryFormatter.Format(CustomDbProfiler.Current.GetCommands()));

                return dbResult;
            }
        }
    }
}
