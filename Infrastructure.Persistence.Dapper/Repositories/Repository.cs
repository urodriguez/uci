using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates;
using Domain.Enums;
using Domain.Predicates;
using Infrastructure.Crosscutting.Logging;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        private readonly ILogService _logService;

        private readonly IDbTransaction _dbTransaction;
        private readonly IDbConnection _dbConnection;

        private readonly QueryFiller _queryFiller;

        public Repository(ILogService logService, IDbTransaction dbTransaction)
        {
            _logService = logService;

            _dbTransaction = dbTransaction;
            _dbConnection = dbTransaction.Connection;

            _queryFiller = new QueryFiller();

            var t = typeof(global::Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
        }

        /// <summary>
        /// Get all elements in repository at least you pass a 'predicate' in order to filter results
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

            var aggregates = _dbConnection.GetList<TAggregateRoot>(dapperPredicate, null, _dbTransaction).ToList();

            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");

            return aggregates;
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
            var aggregate = _dbConnection.Get<TAggregateRoot>(id, _dbTransaction);

            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");

            return aggregate;
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            _dbConnection.Update(aggregateRoot, _dbTransaction);

            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");
        }

        public void Add(TAggregateRoot aggregate)
        {
            _dbConnection.Insert(aggregate, _dbTransaction);//Insert aggregate in db and assign it an Id
            
            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");
        }

        public void Delete(TAggregateRoot aggregate)
        {
            _dbConnection.Delete(aggregate, _dbTransaction);

            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | commands={_queryFiller.Fill(CustomDbProfiler.Current.GetCommands())}");
        }

        public bool Contains(TAggregateRoot aggregate)
        {
            var aggregateInDb = GetById(aggregate.Id);

            _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Element in database obtained, checking if it is not null");

            return aggregateInDb != null;
        }
    }
}
