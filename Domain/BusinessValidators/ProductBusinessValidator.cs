using System;
using System.Linq;
using Domain.Aggregates;
using Domain.Contracts.BusinessValidators;
using Domain.Contracts.Repositories;

namespace Domain.BusinessValidators
{
    public class ProductBusinessValidator : IProductBusinessValidator
    {
        private readonly IProductRepository _productRepository;

        public ProductBusinessValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Validate(Product product)
        {
            if (_productRepository.GetByField(u => u.Name, product.Name).Any()) throw new Exception($"Product with name={product.Name} already exits");
            if (!product.HasValidCode()) throw new Exception($"Product code can not be null or empty");
            if (_productRepository.GetByField(u => u.Code, product.Code).Any()) throw new Exception($"Product with code={product.Code} already exits");
            if (product.Price <= 0) throw new Exception("Product price can not be equal or lower than zero");
        }
    }
}