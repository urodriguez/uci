using System;
using System.Threading.Tasks;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;
using Infrastructure.Crosscutting.Mailing;

namespace Application.BusinessValidators
{
    public class UserBusinessValidator : BusinessValidator<UserDto, User>, IUserBusinessValidator
    {
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly IEmailService _emailService;

        public UserBusinessValidator(IUnitOfWork unitOfWork, IUserPredicateFactory userPredicateFactory, IEmailService emailService) : base(unitOfWork)
        {
            _userPredicateFactory = userPredicateFactory;
            _emailService = emailService;
        }

        protected override async Task ValidateFieldsAsync(UserDto userDto, Guid id)
        {
            var byDistinctIdAndName = _userPredicateFactory.CreateByDistinctIdAndName(id, userDto.Name);
            if (await _unitOfWork.Users.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{AggregateRootName}: name={userDto.Name} already exits");

            if (!_emailService.EmailIsValid(userDto.Email)) throw new BusinessRuleException($"{AggregateRootName}: email={userDto.Email} has invalid email format");

            var byDistinctIdAndEmail = _userPredicateFactory.CreateByDistinctIdAndEmail(id, userDto.Email);
            if (await _unitOfWork.Users.AnyAsync(byDistinctIdAndEmail)) throw new BusinessRuleException($"{AggregateRootName}: email={userDto.Email} already exits");
        }
    }
}