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

        protected override void ValidateFields(ProductDto productDto, Guid? id)
        {
            var predicateGroupForName = new InventAppPredicateGroup<Product>
            {
                Predicates = new List<InventAppPredicate<Product>>
                {
                    new InventAppPredicate<Product>
                    {
                        Field = p => p.Id,
                        Operator = InventAppPredicateOperator.NotEq,
                        Value = id
                    },
                    new InventAppPredicate<Product>
                    {
                        Field = p => p.Name,
                        Operator = InventAppPredicateOperator.Eq,
                        Value = productDto.Name
                    }
                },
                OperatorGroup = InventAppPredicateOperatorGroup.And
            };

            if (_productRepository.Get(predicateGroupForName).Any()) throw new Exception($"{AggregateRootName}: name={productDto.Name} already exits");

            //if (_productRepository.GetByField(u => u.Name, productDto.Name).Any()) throw new Exception($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service
            var predicateGroupForCode = new InventAppPredicateGroup<Product>
            {
                Predicates = new List<InventAppPredicate<Product>>()
                {
                    new InventAppPredicate<Product>
                    {
                        Field = p => p.Id,
                        Operator = InventAppPredicateOperator.NotEq,
                        Value = id
                    },
                    new InventAppPredicate<Product>
                    {
                        Field = p => p.Code,
                        Operator = InventAppPredicateOperator.Eq,
                        Value = productDto.Code
                    }
                },
                OperatorGroup = InventAppPredicateOperatorGroup.And
            };
            if (_productRepository.Get(predicateGroupForCode).Any()) throw new Exception($"{AggregateRootName}: code={productDto.Code} already exits");

            if (!Product.PriceIsEqualOrHigherThanZero(productDto.Price.Value)) throw new Exception($"{AggregateRootName}: price has to be equal or higher than zero");
        }
    }
}