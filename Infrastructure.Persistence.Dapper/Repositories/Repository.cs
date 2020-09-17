using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Logging;
using DapperExtensions;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates;
using Domain.Enums;
using Domain.Predicates;
using MiniProfiler.Integrations;
using StackExchange.Profiling.Data;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly ILogService _logService;
        protected readonly IAppSettingsService _appSettingsService;

        protected readonly IDbTransaction _dbTransaction;
        protected readonly IDbConnection _dbConnection;

        public Repository(ILogService logService, IAppSettingsService appSettingsService, IDbTransaction dbTransaction)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;

            _dbTransaction = dbTransaction;
            _dbConnection = dbTransaction.Connection;

            var t = typeof(global::Dapper.CommandFlags);//dummy code used to import explicitly Dapper - DO NOT DELETE
        }

        /// <summary>
        /// Get all elements in repository at least you pass a 'predicate' in order to filter results
        /// </summary>
        /// <param name="inventAppPredicate">Predicate with filter specifications</param>
        public async Task<IEnumerable<TAggregateRoot>> GetAsync(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null)
        {
            IPredicate dapperPredicate = null;

            if (inventAppPredicate != null)
            {
                dapperPredicate = inventAppPredicate is InventAppPredicateGroup<TAggregateRoot> 
                    ? CreateDapperPredicateGroup((InventAppPredicateGroup<TAggregateRoot>) inventAppPredicate) 
                    : CreateDapperPredicateIndividual((InventAppPredicateIndividual<TAggregateRoot>) inventAppPredicate);
            }

            var aggregates = (await _dbConnection.GetListAsync<TAggregateRoot>(dapperPredicate, null, _dbTransaction)).ToList();

            if (_appSettingsService.Environment.IsDev())
                LogLastQuery();

            return aggregates;
        }

        public async Task<TAggregateRoot> GetFirstAsync(IInventAppPredicate<TAggregateRoot> inventAppPredicate)
        {
            var aggregate = (await GetAsync(inventAppPredicate)).FirstOrDefault();

            return aggregate;
        }

        public async Task<bool> AnyAsync(IInventAppPredicate<TAggregateRoot> inventAppPredicate = null)
        {
            var thereIsAny = (await GetAsync(inventAppPredicate)).Any();

            return thereIsAny;
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
                inventAppPredicate.Operator == ComparisonOperator.NotEq ? Operator.Eq : (Operator) inventAppPredicate.Operator,
                inventAppPredicate.Value,
                inventAppPredicate.Operator == ComparisonOperator.NotEq // InventAppPredicateOperator.NotEq == true => Operator.Eq (not)
            );
        }

        public async Task<TAggregateRoot> GetByIdAsync(Guid id)
        {
            var aggregate = await _dbConnection.GetAsync<TAggregateRoot>(id, _dbTransaction);

            if (_appSettingsService.Environment.IsDev())
                LogLastQuery();

            return aggregate;
        }

        public async Task UpdateAsync(TAggregateRoot aggregateRoot)
        {
            await _dbConnection.UpdateAsync(aggregateRoot, _dbTransaction);

            if (_appSettingsService.Environment.IsDev())
                LogLastQuery();
        }

        public async Task AddAsync(TAggregateRoot aggregate)
        {
            await _dbConnection.InsertAsync(aggregate, _dbTransaction);//Insert aggregate in db and assign it an Id

            if (_appSettingsService.Environment.IsDev())
                LogLastQuery();
        }

        public async Task DeleteAsync(TAggregateRoot aggregate)
        {
            await _dbConnection.DeleteAsync(aggregate, _dbTransaction);

            if (_appSettingsService.Environment.IsDev())
                LogLastQuery();
        }

        public async Task<bool> ContainsAsync(TAggregateRoot aggregate)
        {
            var aggregateInDb = await GetByIdAsync(aggregate.Id);
            return aggregateInDb != null;
        }

        protected void LogLastQuery()
        {
            var profiledDbConnection =  (ProfiledDbConnection)_dbConnection;
            var dbProfiler = (CustomDbProfiler)profiledDbConnection.Profiler;
            var lastCommand = dbProfiler.ProfilerContext.ExecutedCommands.LastOrDefault();

            if (lastCommand == null) return;

            var lastQuery = lastCommand.CommandText;
            foreach (var parameter in lastCommand.Parameters.Keys)
            {
                lastQuery = lastQuery.Replace($"@{parameter}", $"'{lastCommand.Parameters[parameter]}'");
            }

            _logService.LogInfoMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | query={lastQuery}");
        }
    }
}
