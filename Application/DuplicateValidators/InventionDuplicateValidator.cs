using System.Threading.Tasks;
using Application.Contracts.DuplicateValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;

namespace Application.DuplicateValidators
{
    public class InventionDuplicateValidator :  IInventionDuplicateValidator
    {
        private readonly IInventionPredicateFactory _inventionPredicateFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _aggregateRootName;

        public InventionDuplicateValidator(IInventionPredicateFactory inventionPredicateFactory, IUnitOfWork unitOfWork)
        {
            _inventionPredicateFactory = inventionPredicateFactory;
            _unitOfWork = unitOfWork;
            _aggregateRootName = nameof(Invention);
        }

        public async Task ValidateAsync(InventionDto inventionCategoryDto)
        {
            var byDistinctIdAndCode = _inventionPredicateFactory.CreateByDistinctIdAndCode(inventionCategoryDto.Id, inventionCategoryDto.Code);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndCode)) throw new BusinessRuleException($"{_aggregateRootName}: code={inventionCategoryDto.Code} already exits");

            var byDistinctIdAndName = _inventionPredicateFactory.CreateByDistinctIdAndName(inventionCategoryDto.Id, inventionCategoryDto.Name);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{_aggregateRootName}: name={inventionCategoryDto.Name} already exits");

            //TODO: if (!_inventionCodesService.Exists(inventionDto.Code) throw new Exception($"Invention code is invalid"); //invention code needs to be validated on external service
        }
    }
}