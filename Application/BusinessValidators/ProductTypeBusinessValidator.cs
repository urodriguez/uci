using System;
using System.Threading.Tasks;
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

        protected override async Task ValidateFieldsAsync(ProductTypeDto productDto, Guid id)
        {
        }
    }
}