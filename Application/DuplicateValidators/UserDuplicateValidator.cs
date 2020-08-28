using System.Threading.Tasks;
using Application.Contracts.DuplicateValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;

namespace Application.DuplicateValidators
{
    public class UserDuplicateValidator : IUserDuplicateValidator
    {
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _aggregateRootName;

        public UserDuplicateValidator(IUserPredicateFactory userPredicateFactory, IUnitOfWork unitOfWork)
        {
            _userPredicateFactory = userPredicateFactory;
            _unitOfWork = unitOfWork;
            _aggregateRootName = nameof(User);
        }

        public async Task ValidateAsync(UserDto inventionTypeDto)
        {
            var byDistinctIdAndEmail = _userPredicateFactory.CreateByDistinctIdAndEmail(inventionTypeDto.Id, inventionTypeDto.Email);
            if (await _unitOfWork.Users.AnyAsync(byDistinctIdAndEmail)) throw new BusinessRuleException($"{_aggregateRootName}: email={inventionTypeDto.Email} already exits");
        }
    }
}