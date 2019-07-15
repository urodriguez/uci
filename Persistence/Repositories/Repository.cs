using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Persistence.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        private readonly Table<TAggregateRoot> _table;
        internal IDbConnection Connection => new SqlConnection(ConfigurationManager.ConnectionStrings["UciContext"].ConnectionString);

        protected Repository()
        {
            _table = new Table<TAggregateRoot>();
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return cn.Query<TAggregateRoot>("SELECT * FROM " + _table.Name);
            }
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return cn.Query<TAggregateRoot>($"SELECT * FROM {_table.Name} WHERE Id=@Id", new { Id = id }).SingleOrDefault();
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            var query = $"UPDATE {_table.Name} SET {_table.GetColumnsJoinedWithParameters()} WHERE Id=@Id";

            using (var connection = Connection)
            {
                connection.Open();
                connection.Execute(query, aggregateRoot);
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            aggregate.Id = Guid.NewGuid();
            
            var query = $"INSERT INTO {_table.Name} ({_table.GetColumnsJoined()}) VALUES ({_table.GetColumnParameters()})";

            using (var connection = Connection)
            {
                connection.Open();
                connection.Execute(query, aggregate);
            }
        }

        public void Remove(Guid id)
        {
            string sql = $"DELETE FROM {_table.Name} WHERE Id = @Id";

            using (var cn = Connection)
            {
                cn.Open();
                cn.Execute(sql, new { Id = id });
            }
        }

        public void RemoveRange(IEnumerable<Guid> ids)
        {
            string sql = $"DELETE FROM {_table.Name} WHERE Id IN @Ids";

            using (var cn = Connection)
            {
                cn.Open();
                cn.Execute(sql, new { Ids = ids });
            }
        }
    }
}
