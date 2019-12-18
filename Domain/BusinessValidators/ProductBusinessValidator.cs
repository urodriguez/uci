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
        }
    }
}