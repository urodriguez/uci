﻿using System;
using System.Collections.Generic;
using Domain.Aggregates;
using Domain.Contracts.Predicates;
using Domain.Contracts.Predicates.Factories;
using Domain.Enums;

namespace Domain.Predicates.Factories
{
    public class InventionTypePredicateFactory : IInventionTypePredicateFactory
    {
        public IInventAppPredicate<Invention> CreateByDistinctIdAndCode(Guid id, string code)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Invention>(u => u.Code, InventAppPredicateOperator.Eq, code)
                },
                InventAppPredicateOperatorGroup.And
            );
        }

        public IInventAppPredicate<Invention> CreateByDistinctIdAndName(Guid id, string name)
        {
            return new InventAppPredicateGroup<Invention>(
                new List<IInventAppPredicate<Invention>>
                {
                    new InventAppPredicateIndividual<Invention>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Invention>(u => u.Name, InventAppPredicateOperator.Eq, name)
                },
                InventAppPredicateOperatorGroup.And
            );
        }
    }
}