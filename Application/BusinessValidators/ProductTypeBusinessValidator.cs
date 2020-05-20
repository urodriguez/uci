using System;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;

namespace Application.BusinessValidators
{
    public class ProductTypeBusinessValidator : BusinessValidator<ProductTypeDto, ProductType>, IProductTypeBusinessValidator
    {
        public ProductTypeBusinessValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override void ValidateFields(ProductTypeDto productDto, Guid id)
        {
        }
    }
}