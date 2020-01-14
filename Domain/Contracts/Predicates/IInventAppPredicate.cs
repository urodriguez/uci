using Domain.Contracts.Aggregates;

namespace Domain.Contracts.Predicates
{
    public interface IInventAppPredicate<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        void Add(IInventAppPredicate<TAggregateRoot> inventAppPredicate);
    }
}