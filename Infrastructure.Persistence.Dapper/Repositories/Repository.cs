using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DapperExtensions;
using Domain;
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
        public IEnumerable<TAggregateRoot> Get(InventAppPredicate<TAggregateRoot> inventAppPredicate = null)
        {
            IPredicate dapperPredicate = null;

            if (inventAppPredicate != null)
            {
                dapperPredicate = Predicates.Field<TAggregateRoot>(inventAppPredicate.Field, (Operator)inventAppPredicate.Operator, inventAppPredicate.Value);
            }

            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.GetList<TAggregateRoot>(dapperPredicate).ToList();

                _logService.LogInfoMessage($"Repository.ExecuteGet | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return dbResult;
            }
        }

        public IEnumerable<TAggregateRoot> Get(InventAppPredicateGroup<TAggregateRoot> inventAppPredicateGroup)
        {
            var predicates = inventAppPredicateGroup.Predicates.Select(
                inventAppPredicate => Predicates.Field<TAggregateRoot>(inventAppPredicate.Field, (Operator)inventAppPredicate.Operator, inventAppPredicate.Value)
            ).Cast<IPredicate>().ToList();

            var dapperPredicateGroup = new PredicateGroup
            {
                Operator = (GroupOperator)inventAppPredicateGroup.OperatorGroup,
                Predicates = predicates
            };

            using (var sqlConnection = _dbConnectionFactory.GetSqlConnection())
            {
                var dbResult = sqlConnection.GetList<TAggregateRoot>(dapperPredicateGroup).ToList();

                _logService.LogInfoMessage($"Repository.ExecuteGet | commands={CustomDbProfiler.Current.GetCommands()}", MessageType.Query);

                return dbResult;
            }
        }

        /// <summary>
        /// Get all elements in repository whose fields and values match with fieldValues parameter
        /// </summary>
        /// <param name="fieldValues">Fields and values to find</param>
        public IEnumerable<TAggregateRoot> GetByFields(IEnumerable<FieldValue<TAggregateRoot>> fieldValues)
        {
            var predicates = fieldValues.Select(
                fieldValue => new InventAppPredicate<TAggregateRoot> {Field = fieldValue.Field, Operator = InventAppPredicateOperator.Eq, Value = fieldValue.Value}
            ).ToList();

            return Get(new InventAppPredicateGroup<TAggregateRoot>
            {
                OperatorGroup = InventAppPredicateOperatorGroup.Or,
                Predicates = predicates
            });
        }

        /// <summary>
        /// Get all elements in repository whose fields and values match with fieldValues parameter
        /// </summary>
        /// <param name="fieldValues">Fields and values to find</param>
        public IEnumerable<TAggregateRoot> GetByField(Expression<Func<TAggregateRoot, object>> field, object value)
        {
            return Get(new InventAppPredicate<TAggregateRoot>
            {
                Field = field,
                Operator = InventAppPredicateOperator.Eq,
                Value = value
            });
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
