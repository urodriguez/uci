using System;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;
using Domain.Enums;
using Domain.Predicates;
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

        /// <summary>
        /// Get all elements in repository at least you pass a 'predicate' in order to filter some results
        /// </summary>
        /// <param name="inventAppPredicate">Predicate with filter specifications</param>
        public IEnumerable<TAggregateRoot> Get(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null)
        {
            IPredicate dapperPredicate = null;

            if (inventAppPredicate != null)
            {
                if (inventAppPredicate is InventAppPredicateGroup<TAggregateRoot>)
                {
                    var inventAppPredicateGroup = (InventAppPredicateGroup<TAggregateRoot>)inventAppPredicate;
                    dapperPredicate = new PredicateGroup { Operator = (GroupOperator)inventAppPredicateGroup.Operator, Predicates = new List<IPredicate>() };

                    foreach (var inventAppPredicateChild in inventAppPredicateGroup.Predicates)
                    {
                        ((PredicateGroup) dapperPredicate).Predicates.Add(
                            inventAppPredicateChild is InventAppPredicateGroup<TAggregateRoot>
                                ? CreateDapperPredicateGroup((InventAppPredicateGroup<TAggregateRoot>)inventAppPredicateChild)
                                : CreateDapperPredicate((InventAppPredicate<TAggregateRoot>)inventAppPredicateChild)
                        );
                    }
                }
                else
                {
                    var inventAppPredicateBasic = (InventAppPredicate<TAggregateRoot>)inventAppPredicate;
                    dapperPredicate = Predicates.Field<TAggregateRoot>(inventAppPredicateBasic.Field, (Operator)inventAppPredicateBasic.Operator, inventAppPredicateBasic.Value);
                }
            }

            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.GetList<TAggregateRoot>(dapperPredicate).ToList();

                _logService.LogInfoMessage($"Repository.ExecuteGet | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return dbResult;
            }
        }

        private IPredicate CreateDapperPredicateGroup(InventAppPredicateGroup<TAggregateRoot> inventAppPredicate)
        {
            var pg = new PredicateGroup { Operator = (GroupOperator)inventAppPredicate.Operator, Predicates = new List<IPredicate>() };

            foreach (var predicate in inventAppPredicate.Predicates)
            {
                CreateDapperPredicate((InventAppPredicate<TAggregateRoot>)predicate);
            }

            return pg;
        }

        private IPredicate CreateDapperPredicate(InventAppPredicate<TAggregateRoot> inventAppPredicate)
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
            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                sqlConnection.Insert(aggregate);//Insert aggregate in db and assign it an Id
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
    }
}
