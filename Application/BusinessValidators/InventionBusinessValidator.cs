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
    public class InventionBusinessValidator : BusinessValidator<InventionDto, Invention>, IInventionBusinessValidator
    {
        private readonly IInventionPredicateFactory _inventionPredicateFactory;

        public InventionBusinessValidator(IUnitOfWork unitOfWork, IInventionPredicateFactory inventionPredicateFactory) : base(unitOfWork)
        {
            _inventionPredicateFactory = inventionPredicateFactory;
        }

        protected override async Task ValidateFieldsAsync(InventionDto inventionDto, Guid id)
        {
            var byDistinctIdAndName = _inventionPredicateFactory.CreateByDistinctIdAndName(id, inventionDto.Name);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{AggregateRootName}: name={inventionDto.Name} already exits");

            //TODO: if (!_inventionCodesService.Exists(inventionDto.Code) throw new Exception($"Invention code is invalid"); //invention code needs to be validated on external service

            var byDistinctIdAndCode = _inventionPredicateFactory.CreateByDistinctIdAndCode(id, inventionDto.Code);
            if (await _unitOfWork.Inventions.AnyAsync(byDistinctIdAndCode)) throw new BusinessRuleException($"{AggregateRootName}: code={inventionDto.Code} already exits");

            Invention.ValidateCode(inventionDto.Code);
            Invention.ValidatePrice(inventionDto.Price.Value);
        }
    }
}