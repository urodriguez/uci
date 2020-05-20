using System;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;

namespace Application.BusinessValidators
{
    public class UserBusinessValidator : BusinessValidator<UserDto, User>, IUserBusinessValidator
    {
        private readonly IUserPredicateFactory _userPredicateFactory;

        public UserBusinessValidator(IUnitOfWork unitOfWork, IUserPredicateFactory userPredicateFactory) : base(unitOfWork)
        {
            _userPredicateFactory = userPredicateFactory;
        }

        protected override void ValidateFields(UserDto userDto, Guid id)
        {
            var byDistinctIdAndName = _userPredicateFactory.CreateByDistinctIdAndName(id, userDto.Name);
            if (_unitOfWork.Users.Get(byDistinctIdAndName).Any()) throw new BusinessRuleException($"{AggregateRootName}: name={userDto.Name} already exits");

            if (!User.EmailIsValid(userDto.Email)) throw new BusinessRuleException($"{AggregateRootName}: email={userDto.Email} has invalid email format");

            var byDistinctIdAndEmail = _userPredicateFactory.CreateByDistinctIdAndEmail(id, userDto.Email);
            if (_unitOfWork.Users.Get(byDistinctIdAndEmail).Any()) throw new BusinessRuleException($"{AggregateRootName}: email={userDto.Email} already exits");
        }
    }
}