using System;
using System.Threading.Tasks;
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

        protected override async Task ValidateFieldsAsync(UserDto inventionDto, Guid id)
        {
            var byDistinctIdAndName = _userPredicateFactory.CreateByDistinctIdAndName(id, inventionDto.Name);
            if (await _unitOfWork.Users.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{AggregateRootName}: name={inventionDto.Name} already exits");

            if (!User.EmailIsValid(inventionDto.Email)) throw new BusinessRuleException($"{AggregateRootName}: email={inventionDto.Email} has invalid email format");

            var byDistinctIdAndEmail = _userPredicateFactory.CreateByDistinctIdAndEmail(id, inventionDto.Email);
            if (await _unitOfWork.Users.AnyAsync(byDistinctIdAndEmail)) throw new BusinessRuleException($"{AggregateRootName}: email={inventionDto.Email} already exits");
        }
    }
}