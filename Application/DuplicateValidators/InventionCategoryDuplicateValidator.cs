using System.Threading.Tasks;
using Application.Contracts.DuplicateValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;

namespace Application.DuplicateValidators
{
    public class InventionCategoryDuplicateValidator : IInventionCategoryDuplicateValidator
    {
        private readonly IInventionCategoryPredicateFactory _inventionCategoryPredicateFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _aggregateRootName;

        public InventionCategoryDuplicateValidator(IInventionCategoryPredicateFactory inventionCategoryPredicateFactory, IUnitOfWork unitOfWork)
        {
            _inventionCategoryPredicateFactory = inventionCategoryPredicateFactory;
            _unitOfWork = unitOfWork;
            _aggregateRootName = nameof(InventionCategory);
        }

        public async Task ValidateAsync(InventionCategoryDto inventionCategoryDto)
        {
            var byDistinctIdAndName = _inventionCategoryPredicateFactory.CreateByDistinctIdAndName(inventionCategoryDto.Id, inventionCategoryDto.Name);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{_aggregateRootName}: name={inventionCategoryDto.Name} already exits");

            var byDistinctIdAndCode = _inventionCategoryPredicateFactory.CreateByDistinctIdAndCode(inventionCategoryDto.Id, inventionCategoryDto.Code);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndCode)) throw new BusinessRuleException($"{_aggregateRootName}: code={inventionCategoryDto.Code} already exits");
        }
    }
}