using Domain.Contracts.Aggregates;

namespace Domain.Contracts.Predicates.Factories
{
    public interface IPredicateFactory<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        
    }
}