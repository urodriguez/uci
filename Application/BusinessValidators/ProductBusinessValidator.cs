using System;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.BusinessValidators
{
    public class ProductBusinessValidator : BusinessValidator<ProductDto, Product>, IProductBusinessValidator
    {
        private readonly IProductRepository _productRepository;

        public ProductBusinessValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        protected override void ValidateFields(ProductDto productDto)
        {
            ValidateRequiredFields(productDto);

            if (_productRepository.GetByField(u => u.Name, productDto.Name).Any()) throw new Exception($"{AggregateRootName}: name={productDto.Name} already exits");

            //TODO: if (!_productCodesService.Exists(productDto.Code) throw new Exception($"Product code is invalid"); //product code needs to be validated on external service
            if (_productRepository.GetByField(u => u.Code, productDto.Code).Any()) throw new Exception($"{AggregateRootName}: code={productDto.Code} already exits");

            if (!Product.PriceIsEqualOrHigherThanZero(productDto.Price.Value)) throw new Exception($"{AggregateRootName}: price has to be equal or higher than zero");
        }
    }
}