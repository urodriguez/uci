using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Domain.Aggregates;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Persistence.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        private readonly string _tableName;
        protected IList<TAggregateRoot> AggregateRoots { get; set; }
      internal IDbConnection Connection
      {
        get
        {
          return new SqlConnection(ConfigurationManager.ConnectionStrings["UciContext"].ConnectionString);
        }
      }

      public Repository(string tableName)
      {
        _tableName = tableName;
      }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            return AggregateRoots;
        }

        public TAggregateRoot GetById(int id)
        {
          TAggregateRoot item = default(TAggregateRoot);

          using (IDbConnection cn = Connection)
          {
            cn.Open();
            item = cn.Query<TAggregateRoot>("SELECT * FROM " + _tableName + " WHERE ID=@ID", new { ID = id }).SingleOrDefault();
          }

          return item;
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            var aggregateRootFound = AggregateRoots.FirstOrDefault(ar => ar.Id == aggregateRoot.Id);

            if (aggregateRootFound == null) return;

            aggregateRootFound.Id = aggregateRoot.Id;
            //aggregateRootFound.Name = aggregateRoot.Name;
            //aggregateRootFound.Category = aggregateRoot.Category;
            //aggregateRootFound.Price = aggregateRoot.Price;
        }

        public void Remove(int id)
        {
            var aggregateRootFound = AggregateRoots.FirstOrDefault(ar => ar.Id == id);
            if (aggregateRootFound == null) return;

            AggregateRoots.Remove(aggregateRootFound);
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = new Random().Next(0, 1000);
            AggregateRoots.Add(aggregate);
        }
    }
}
