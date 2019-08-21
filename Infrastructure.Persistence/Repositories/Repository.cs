using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        protected Repository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
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

        public void Delete(Guid id)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregate = sqlConnection.Get<TAggregateRoot>(id);
                sqlConnection.Delete(aggregate);
            }
        }
    }
}
