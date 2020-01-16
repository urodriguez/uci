using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates;
using Domain.Enums;
using Domain.Predicates;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogService _logService;
        private readonly QueryFiller _queryFiller;

        protected Repository(IDbConnectionFactory dbConnectionFactory, ILogService logService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logService = logService;
            _queryFiller = new QueryFiller();

            var x = typeof(global::Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
            _logService.LogInfoMessage($"Repository.Ctor | Repository created | ORM={x}");
        }

        /// <summary>
        /// Get all elements in repository at least you pass a 'predicate' in order to filter some results
        /// </summary>
        /// <param name="inventAppPredicate">Predicate with filter specifications</param>
        public IEnumerable<TAggregateRoot> Get(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null)
        {
            IPredicate dapperPredicate = null;

            if (inventAppPredicate != null)
            {
                dapperPredicate = inventAppPredicate is InventAppPredicateGroup<TAggregateRoot> 
                    ? CreateDapperPredicateGroup((InventAppPredicateGroup<TAggregateRoot>) inventAppPredicate) 
                    : CreateDapperPredicateIndividual((InventAppPredicateIndividual<TAggregateRoot>) inventAppPredicate);
            }

            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.GetList<TAggregateRoot>(dapperPredicate).ToList();

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");

                return dbResult;
            }
        }

        private IPredicate CreateDapperPredicateGroup(InventAppPredicateGroup<TAggregateRoot> inventAppPredicate)
        {
            var dapperPredicate = new PredicateGroup { Operator = (GroupOperator)inventAppPredicate.Operator, Predicates = new List<IPredicate>() };

            foreach (var inventAppPredicateChild in inventAppPredicate.Predicates)
            {
                dapperPredicate.Predicates.Add(
                    inventAppPredicateChild is InventAppPredicateGroup<TAggregateRoot>
                        ? CreateDapperPredicateGroup((InventAppPredicateGroup<TAggregateRoot>)inventAppPredicateChild)
                        : CreateDapperPredicateIndividual((InventAppPredicateIndividual<TAggregateRoot>)inventAppPredicateChild)
                );
            }

            return dapperPredicate;
        }

        private IPredicate CreateDapperPredicateIndividual(InventAppPredicateIndividual<TAggregateRoot> inventAppPredicate)
        {
            return Predicates.Field<TAggregateRoot>(
                inventAppPredicate.Field,
                inventAppPredicate.Operator == InventAppPredicateOperator.NotEq ? Operator.Eq : (Operator) inventAppPredicate.Operator,
                inventAppPredicate.Value,
                inventAppPredicate.Operator == InventAppPredicateOperator.NotEq // InventAppPredicateOperator.NotEq == true => Operator.Eq (not)
            );
        }

        public TAggregateRoot GetById(Guid id)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var aggregate = sqlConnection.Get<TAggregateRoot>(id);

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");

                return aggregate;
            }
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Update(aggregateRoot);
                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");
            }
        }

        public void Add(TAggregateRoot aggregate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Insert(aggregate);//Insert aggregate in db and assign it an Id
                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");
            }
        }

        public void Delete(TAggregateRoot aggregate)
        {
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Delete(aggregate);
                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");
            }
        }

        public bool Contains(TAggregateRoot aggregate)
        {
            var aggregateInDb = GetById(aggregate.Id);

            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Element in database obtained, checking if it is not null");

            return aggregateInDb != null;
        }
    }
}
