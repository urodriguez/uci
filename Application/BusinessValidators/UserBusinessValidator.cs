using System;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Repositories;

namespace Application.BusinessValidators
{
    public class UserBusinessValidator : BusinessValidator<UserDto, User>, IUserBusinessValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPredicateFactory _userPredicateFactory;

        public UserBusinessValidator(IUserRepository userRepository, IUserPredicateFactory userPredicateFactory)
        {
            _userRepository = userRepository;
            _userPredicateFactory = userPredicateFactory;
        }

        protected override void ValidateFields(UserDto userDto, Guid id)
        {
            var byDistinctIdAndName = _userPredicateFactory.CreateByDistinctIdAndName(id, userDto.Name);
            if (_userRepository.Get(byDistinctIdAndName).Any()) throw new Exception($"{AggregateRootName}: name={userDto.Name} already exits");

            if (!User.EmailIsValid(userDto.Email)) throw new Exception($"{AggregateRootName}: email={userDto.Email} has invalid email format");

            var byDistinctIdAndEmail = _userPredicateFactory.CreateByDistinctIdAndEmail(id, userDto.Email);
            if (_userRepository.Get(byDistinctIdAndEmail).Any()) throw new Exception($"{AggregateRootName}: email={userDto.Email} already exits");
        }
    }
}