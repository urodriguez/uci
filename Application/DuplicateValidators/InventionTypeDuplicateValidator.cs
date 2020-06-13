using System.Threading.Tasks;
using Application.Contracts.DuplicateValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;

namespace Application.DuplicateValidators
{
    public class InventionTypeDuplicateValidator : IInventionTypeDuplicateValidator
    {
        private readonly IInventionTypePredicateFactory _inventionTypePredicateFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _aggregateRootName;

        public InventionTypeDuplicateValidator(IInventionTypePredicateFactory inventionTypePredicateFactory, IUnitOfWork unitOfWork)
        {
            _inventionTypePredicateFactory = inventionTypePredicateFactory;
            _unitOfWork = unitOfWork;
            _aggregateRootName = nameof(InventionType);
        }

        public async Task ValidateAsync(InventionTypeDto inventionTypeDto)
        {
            var byDistinctIdAndName = _inventionTypePredicateFactory.CreateByDistinctIdAndName(inventionTypeDto.Id, inventionTypeDto.Name);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{_aggregateRootName}: name={inventionTypeDto.Name} already exits");

            var byDistinctIdAndCode = _inventionTypePredicateFactory.CreateByDistinctIdAndCode(inventionTypeDto.Id, inventionTypeDto.Code);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndCode)) throw new BusinessRuleException($"{_aggregateRootName}: code={inventionTypeDto.Code} already exits");
        }
    }
}