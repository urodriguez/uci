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
    public class ProductBusinessValidator : BusinessValidator<ProductDto, Product>, IProductBusinessValidator
    {
        private readonly IProductPredicateFactory _productPredicateFactory;

        public ProductBusinessValidator(IUnitOfWork unitOfWork, IProductPredicateFactory productPredicateFactory) : base(unitOfWork)
        {
            _productPredicateFactory = productPredicateFactory;
        }

        protected override void ValidateFields(ProductDto productDto, Guid id)
        {
            var byDistinctIdAndName = _productPredicateFactory.CreateByDistinctIdAndName(id, productDto.Name);
            if (_unitOfWork.Products.Get(byDistinctIdAndName).Any()) throw new BusinessRuleException($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service

            var byDistinctIdAndCode = _productPredicateFactory.CreateByDistinctIdAndCode(id, productDto.Code);
            if (_unitOfWork.Products.Get(byDistinctIdAndCode).Any()) throw new BusinessRuleException($"{AggregateRootName}: code={productDto.Code} already exits");

            Product.ValidatePrice(productDto.Price.Value);
        }
    }
}