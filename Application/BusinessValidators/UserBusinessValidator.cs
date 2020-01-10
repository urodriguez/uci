﻿using System;
using System.Collections.Generic;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Domain.Enums;
using Domain.Predicates;

namespace Application.BusinessValidators
{
    public class UserBusinessValidator : BusinessValidator<UserDto, User>, IUserBusinessValidator
    {
        private readonly IUserRepository _userRepository;

        public UserBusinessValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override void ValidateFields(UserDto userDto, Guid id)
        {
            var byDistinctIdAndName = new InventAppPredicateGroup<User>
            {
                Predicates = new List<IInventAppPredicate<User>>
                {
                    new InventAppPredicate<User>
                    {
                        Field = u => u.Id,
                        Operator = InventAppPredicateOperator.NotEq,
                        Value = id
                    },
                    new InventAppPredicate<User>
                    {
                        Field = u => u.Name,
                        Operator = InventAppPredicateOperator.Eq,
                        Value = userDto.Name
                    }
                },
                Operator = InventAppPredicateOperatorGroup.And
            };
            if (_userRepository.Get(byDistinctIdAndName).Any()) throw new Exception($"{AggregateRootName}: name={userDto.Name} already exits");

            if (!User.EmailIsValid(userDto.Email)) throw new Exception($"{AggregateRootName}: email={userDto.Email} has invalid email format");

            var byDistinctIdAndEmail = new InventAppPredicateGroup<User>
            {
                Predicates = new List<IInventAppPredicate<User>>
                {
                    new InventAppPredicate<User>
                    {
                        Field = u => u.Id,
                        Operator = InventAppPredicateOperator.NotEq,
                        Value = id
                    },
                    new InventAppPredicate<User>
                    {
                        Field = u => u.Email,
                        Operator = InventAppPredicateOperator.Eq,
                        Value = userDto.Email
                    }
                },
                Operator = InventAppPredicateOperatorGroup.And
            };
            if (_userRepository.Get(byDistinctIdAndEmail).Any()) throw new Exception($"{AggregateRootName}: email={userDto.Email} already exits");
        }
    }
}