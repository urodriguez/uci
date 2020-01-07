using System;
using Application.Dtos;
using AutoMapper;
using Domain.Contracts.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public abstract class AggregateProfile : Profile
    {
        protected static Action<IMemberConfigurationExpression<TDto, TAggregateRoot, object>> GetConfiguredOptions<TDto, TAggregateRoot>()  where TAggregateRoot : IAggregateRoot where TDto : IDto
        {
            return memberOptions =>
            {
                //Iterate all members and map IF 'true' is returned 
                memberOptions.Condition((s, d, sourceValue, destValue, rc) =>
                {
                    try
                    {
                        var sourceGuidType = new Guid(sourceValue.ToString());
                        if (sourceGuidType == Guid.Empty) return false;
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }

                    if (sourceValue == null) return false;

                    return true;
                });
            };
        }
    }
}