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
            var byDistinctIdAndName = new InventAppPredicateGroup<Product>
            {
                Predicates = new List<IInventAppPredicate<Product>>
                {
                    new InventAppPredicate<Product>
                    {
                        Field = u => u.Id,
                        Operator = InventAppPredicateOperator.NotEq,
                        Value = id
                    },
                    new InventAppPredicate<Product>
                    {
                        Field = u => u.Name,
                        Operator = InventAppPredicateOperator.Eq,
                        Value = productDto.Name
                    }
                },
                Operator = InventAppPredicateOperatorGroup.And
            };
            if (_productRepository.Get(byDistinctIdAndName).Any()) throw new Exception($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service

            var byDistinctIdAndCode = new InventAppPredicateGroup<Product>
            {
                Predicates = new List<IInventAppPredicate<Product>>
                {
                    new InventAppPredicate<Product>
                    {
                        Field = u => u.Id,
                        Operator = InventAppPredicateOperator.NotEq,
                        Value = id
                    },
                    new InventAppPredicate<Product>
                    {
                        Field = u => u.Code,
                        Operator = InventAppPredicateOperator.Eq,
                        Value = productDto.Code
                    }
                },
                Operator = InventAppPredicateOperatorGroup.And
            };
            if (_productRepository.Get(byDistinctIdAndCode).Any()) throw new Exception($"{AggregateRootName}: code={productDto.Code} already exits");

            if (!Product.PriceIsEqualOrHigherThanZero(productDto.Price.Value)) throw new Exception($"{AggregateRootName}: price has to be equal or higher than zero");
        }
    }
}