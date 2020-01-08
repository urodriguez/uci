using System;
using System.Linq;
using System.Reflection;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Attributes;
using Domain.Contracts.Aggregates;

namespace Application.BusinessValidators
{
    public abstract class BusinessValidator<TDto, TAggregateRoot> : IBusinessValidator<TDto> where TDto : IDto where TAggregateRoot : IAggregateRoot
    {
        protected readonly string AggregateRootName;

        protected BusinessValidator()
        {
            AggregateRootName = typeof(TAggregateRoot).Name;
        }

        public void Validate(TDto dto, Guid id = default(Guid)) //default(Guid) == Guid.Empty
        {
            ValidateRequiredFields(dto);
            ValidateFields(dto, id);
        }

        protected void ValidateRequiredFields(TDto dto)
        {
            foreach (PropertyInfo dtoProperty in typeof(TDto).GetProperties())
            {
                var property = typeof(TAggregateRoot).GetProperties().SingleOrDefault(p => p.Name == dtoProperty.Name);

                if (property != null && Attribute.IsDefined(property, typeof(Required)))//property exits on aggregate and is required
                {
                    var propertyValue = dtoProperty.GetValue(dto, null);

                    if (propertyValue == null) throw new Exception($"{AggregateRootName}: Property '{property.Name}' is required");

                    //case 'string'
                    if (typeof(string).IsAssignableFrom(dtoProperty.PropertyType) && propertyValue.ToString() == string.Empty)
                    {
                        throw new Exception($"{AggregateRootName}: Property '{dtoProperty.Name}' is required");
                    }
                }
            }
        }

        protected abstract void ValidateFields(TDto dto, Guid id);
    }
}