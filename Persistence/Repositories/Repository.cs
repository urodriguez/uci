using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Aggregates;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Persistence.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {

        protected IList<TAggregateRoot> AggregateRoots { get; set; }

        protected Repository(IList<TAggregateRoot> aggregateRoots)
        {
            AggregateRoots = aggregateRoots;
        }

        public IEnumerable<TAggregateRoot> GetAll()
        {
            return AggregateRoots;
        }

        public TAggregateRoot GetById(int id)
        {
            //open db connection
            return AggregateRoots.FirstOrDefault(ar => ar.Id == id);
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
