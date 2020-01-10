using Domain.Contracts.Aggregates;

namespace Domain.Predicates
{
    public interface IInventAppPredicate<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
    }
}