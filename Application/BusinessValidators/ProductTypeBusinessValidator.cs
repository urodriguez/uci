using System;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Application.BusinessValidators
{
    public class ProductTypeBusinessValidator : BusinessValidator<ProductTypeDto, ProductType>, IProductTypeBusinessValidator
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypeBusinessValidator(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        protected override void ValidateFields(ProductTypeDto productDto, Guid id)
        {
        }
    }
}