using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            _logService.LogInfoMessage($"Repository.Ctor | Repository created | ORM={x}");
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregates = sqlConnection.GetList<TAggregateRoot>().ToList();

                _logService.LogInfoMessage($"Repository.GetAll | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return aggregates;
            }
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregate = sqlConnection.Get<TAggregateRoot>(id);

                _logService.LogInfoMessage($"Repository.GetById | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return aggregate;
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Update(aggregateRoot);
                _logService.LogInfoMessage($"Repository.Update | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = Guid.NewGuid();
            
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Insert(aggregate);
                _logService.LogInfoMessage($"Repository.Add | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);
            }
        }

        public void Delete(TAggregateRoot aggregate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Delete(aggregate);
                _logService.LogInfoMessage($"Repository.Delete | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);
            }
        }

        public bool Contains(TAggregateRoot aggregate)
        {
            var aggregateInDb = GetById(aggregate.Id);

            _logService.LogInfoMessage($"Repository.Contains | Element in database obtained, checking if it is not null", MessageType.Query);

            return aggregateInDb != null;
        }

        protected IEnumerable<TAggregateRoot> ExecuteGetList(IPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResultList = sqlConnection.GetList<TAggregateRoot>(predicate).ToList();

                _logService.LogInfoMessage($"Repository.ExecuteGetList | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return dbResultList;
            }
        }

        protected TAggregateRoot ExecuteGet(IPredicate predicate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.GetList<TAggregateRoot>(predicate).FirstOrDefault();

                _logService.LogInfoMessage($"Repository.ExecuteGet | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return dbResult;
            }
        }
    }
}
