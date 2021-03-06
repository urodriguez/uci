﻿using System;
using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Contracts.Predicates;
using Domain.Contracts.Predicates.Factories;
using Domain.Enums;

namespace Domain.Predicates.Factories
{
    public class UserPredicateFactory : IUserPredicateFactory
    {
        public IInventAppPredicate<User> CreateByDistinctIdAndEmail(Guid id, string email)
        {
            return new InventAppPredicateGroup<User>(
                new List<IInventAppPredicate<User>>
                {
                    new InventAppPredicateIndividual<User>(u => u.Id, ComparisonOperator.NotEq, id),
                    new InventAppPredicateIndividual<User>(u => u.Email, ComparisonOperator.Eq, email)
                },
                ComparisonOperatorGroup.And
            );
        }

        public IInventAppPredicate<User> CreateByEmail(string email)
        {
            return new InventAppPredicateIndividual<User>(u => u.Email, ComparisonOperator.Eq, email);
        }
    }
}