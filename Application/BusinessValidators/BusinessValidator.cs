using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Attributes;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Exceptions;

namespace Application.BusinessValidators
{
    public abstract class BusinessValidator<TDto, TAggregateRoot> : IBusinessValidator<TDto> where TDto : IDto where TAggregateRoot : IAggregateRoot
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly string AggregateRootName;

        protected BusinessValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            AggregateRootName = typeof(TAggregateRoot).Name;
        }

        public async Task ValidateAsync(TDto dto, Guid id = default(Guid)) //default(Guid) == Guid.Empty
        {
            ValidateRequiredFields(dto);
            await ValidateFieldsAsync(dto, id);
        }

        protected void ValidateRequiredFields(TDto dto)
        {
            foreach (PropertyInfo dtoProperty in typeof(TDto).GetProperties())
            {
                var property = typeof(TAggregateRoot).GetProperties().SingleOrDefault(p => p.Name == dtoProperty.Name);

                if (property != null && Attribute.IsDefined(property, typeof(Required)))//property exits on aggregate and is required
                {
                    var propertyValue = dtoProperty.GetValue(dto, null);

                    if (propertyValue == null) throw new BusinessRuleException($"{AggregateRootName}: Property '{property.Name}' is required");

                    //case 'string'
                    if (typeof(string).IsAssignableFrom(dtoProperty.PropertyType) && propertyValue.ToString() == string.Empty)
                    {
                        throw new BusinessRuleException($"{AggregateRootName}: Property '{dtoProperty.Name}' is required");
                    }
                }
            }
        }

        protected abstract Task ValidateFieldsAsync(TDto dto, Guid id);
    }
}