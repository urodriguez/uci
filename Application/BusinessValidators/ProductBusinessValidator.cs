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
    public class ProductBusinessValidator : BusinessValidator<ProductDto, Product>, IProductBusinessValidator
    {
        private readonly IProductPredicateFactory _productPredicateFactory;

        public ProductBusinessValidator(IUnitOfWork unitOfWork, IProductPredicateFactory productPredicateFactory) : base(unitOfWork)
        {
            _productPredicateFactory = productPredicateFactory;
        }

        protected override async Task ValidateFieldsAsync(ProductDto productDto, Guid id)
        {
            var byDistinctIdAndName = _productPredicateFactory.CreateByDistinctIdAndName(id, productDto.Name);
            if (await _unitOfWork.Products.AnyAsync(byDistinctIdAndName)) throw new BusinessRuleException($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service

            var byDistinctIdAndCode = _productPredicateFactory.CreateByDistinctIdAndCode(id, productDto.Code);
            if (await _unitOfWork.Products.AnyAsync(byDistinctIdAndCode)) throw new BusinessRuleException($"{AggregateRootName}: code={productDto.Code} already exits");

            Product.ValidateCode(productDto.Code);
            Product.ValidatePrice(productDto.Price.Value);
        }
    }
}