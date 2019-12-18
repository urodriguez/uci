using Domain.Aggregates;
using Domain.Contracts.BusinessValidators;
using Domain.Contracts.Repositories;

namespace Domain.BusinessValidators
{
    public class ProductTypeBusinessValidator : IProductTypeBusinessValidator
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypeBusinessValidator(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        public void Validate(ProductType productType)
        {
        }
    }
}