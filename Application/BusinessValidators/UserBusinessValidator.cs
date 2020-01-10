using System;
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
            var byDistinctIdAndName = new InventAppPredicateGroup<User>(
                new List<IInventAppPredicate<User>>
                {
                    new InventAppPredicateIndividual<User>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<User>(u => u.Name, InventAppPredicateOperator.Eq, userDto.Name)
                },
                InventAppPredicateOperatorGroup.And
            );
            if (_userRepository.Get(byDistinctIdAndName).Any()) throw new Exception($"{AggregateRootName}: name={userDto.Name} already exits");

            if (!User.EmailIsValid(userDto.Email)) throw new Exception($"{AggregateRootName}: email={userDto.Email} has invalid email format");

            var byDistinctIdAndEmail = new InventAppPredicateGroup<User>(
                new List<IInventAppPredicate<User>>
                {
                    new InventAppPredicateIndividual<User>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<User>(u => u.Email, InventAppPredicateOperator.Eq, userDto.Email)
                },
                InventAppPredicateOperatorGroup.And
            );
            if (_userRepository.Get(byDistinctIdAndEmail).Any()) throw new Exception($"{AggregateRootName}: email={userDto.Email} already exits");
        }
    }
}