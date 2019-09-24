using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Logging;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogService _logService;

        protected Repository(IDbConnectionFactory dbConnectionFactory, ILogService logService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logService = logService;

            var x = typeof(global::Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
            _logService.QueueInfoMessage($"Repository created for {x} ORM");
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregates = sqlConnection.GetList<TAggregateRoot>().ToList();

                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);

                return aggregates;
            }
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregate = sqlConnection.Get<TAggregateRoot>(id);

                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);

                return aggregate;
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Update(aggregateRoot);
                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = Guid.NewGuid();
            
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Insert(aggregate);
                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);
            }
        }

        public void Delete(TAggregateRoot aggregate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Delete(aggregate);
                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);
            }
        }

        protected IEnumerable<TAggregateRoot> ExecuteGetList(IPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResultList = sqlConnection.GetList<TAggregateRoot>(predicate).ToList();

                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);

                return dbResultList;
            }
        }

        protected TAggregateRoot ExecuteGet(IPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.Get<TAggregateRoot>(predicate);

                _logService.QueueInfoMessage(CustomDbProfiler.Current.GetCommands(), MessageType.Query);

                return dbResult;
            }
        }
    }
}
