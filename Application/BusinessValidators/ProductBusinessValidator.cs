using System;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Exceptions;

namespace Application.BusinessValidators
{
    public class ProductBusinessValidator : BusinessValidator<ProductDto, Product>, IProductBusinessValidator
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPredicateFactory _productPredicateFactory;

        public ProductBusinessValidator(IProductRepository productRepository, IProductPredicateFactory productPredicateFactory)
        {
            _productRepository = productRepository;
            _productPredicateFactory = productPredicateFactory;
        }

        protected override void ValidateFields(ProductDto productDto, Guid id)
        {
            var byDistinctIdAndName = _productPredicateFactory.CreateByDistinctIdAndName(id, productDto.Name);
            if (_productRepository.Get(byDistinctIdAndName).Any()) throw new BusinessRuleException($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service

            var byDistinctIdAndCode = _productPredicateFactory.CreateByDistinctIdAndCode(id, productDto.Code);
            if (_productRepository.Get(byDistinctIdAndCode).Any()) throw new BusinessRuleException($"{AggregateRootName}: code={productDto.Code} already exits");

            Product.ValidatePrice(productDto.Price.Value);
        }
    }
}