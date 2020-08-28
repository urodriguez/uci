using System;
using Domain.Aggregates;

namespace Domain.Contracts.Predicates.Factories
{
    public interface IUserPredicateFactory : IPredicateFactory<User>
    {
        IInventAppPredicate<User> CreateByDistinctIdAndEmail(Guid id, string email);
        IInventAppPredicate<User> CreateByEmail(string email);
    }
}