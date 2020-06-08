using System;
using System.Threading.Tasks;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;

namespace Application.BusinessValidators
{
    public class InventionTypeBusinessValidator : BusinessValidator<InventionTypeDto, InventionType>, IInventionTypeBusinessValidator
    {
        public InventionTypeBusinessValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override async Task ValidateFieldsAsync(InventionTypeDto inventionDto, Guid id)
        {
        }
    }
}