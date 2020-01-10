using System;
using System.Collections.Generic;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Domain.Enums;
using Domain.Predicates;

namespace Application.BusinessValidators
{
    public class ProductBusinessValidator : BusinessValidator<ProductDto, Product>, IProductBusinessValidator
    {
        private readonly IProductRepository _productRepository;

        public ProductBusinessValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        protected override void ValidateFields(ProductDto productDto, Guid id)
        {
            var byDistinctIdAndName = new InventAppPredicateGroup<Product>(
                new List<IInventAppPredicate<Product>>
                {
                    new InventAppPredicateIndividual<Product>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Product>(u => u.Name, InventAppPredicateOperator.Eq, productDto.Name)
                },
                InventAppPredicateOperatorGroup.And
            );
            if (_productRepository.Get(byDistinctIdAndName).Any()) throw new Exception($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service

            var byDistinctIdAndCode = new InventAppPredicateGroup<Product>(
                new List<IInventAppPredicate<Product>>
                {
                    new InventAppPredicateIndividual<Product>(u => u.Id, InventAppPredicateOperator.NotEq, id),
                    new InventAppPredicateIndividual<Product>(u => u.Code, InventAppPredicateOperator.Eq, productDto.Code)
                },
                InventAppPredicateOperatorGroup.And
            );
            if (_productRepository.Get(byDistinctIdAndCode).Any()) throw new Exception($"{AggregateRootName}: code={productDto.Code} already exits");

            if (!Product.PriceIsEqualOrHigherThanZero(productDto.Price.Value)) throw new Exception($"{AggregateRootName}: price has to be equal or higher than zero");
        }
    }
}