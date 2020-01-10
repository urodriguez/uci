using Domain.Contracts.Aggregates;

namespace Domain.Predicates
{
    public interface IInventAppPredicate<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        void Add(IInventAppPredicate<TAggregateRoot> inventAppPredicate);
    }
}