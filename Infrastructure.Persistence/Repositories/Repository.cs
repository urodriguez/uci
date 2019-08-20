using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        internal IDbConnection Connection => new SqlConnection(ConfigurationManager.ConnectionStrings["InventappContext"].ConnectionString);

        protected Repository()
        {
            var x = typeof(Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return cn.GetList<TAggregateRoot>().ToList();
            }
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return cn.Get<TAggregateRoot>(id);
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            using (var connection = Connection)
            {
                connection.Open();
                connection.Update(aggregateRoot);
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = Guid.NewGuid();
            
            using (var connection = Connection)
            {
                connection.Open();
                connection.Insert(aggregate);
            }
        }

        public void Delete(Guid id)
        {
            using (var cn = Connection)
            {
                cn.Open();
                var aggregate = cn.Get<TAggregateRoot>(id);
                cn.Delete(aggregate);
            }
        }
    }
}
