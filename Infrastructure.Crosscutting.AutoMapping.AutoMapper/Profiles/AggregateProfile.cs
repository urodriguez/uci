using System;
using Application.Dtos;
using AutoMapper;
using Domain.Contracts.Aggregates;

namespace Infrastructure.Crosscutting.AutoMapping.AutoMapper.Profiles
{
    public abstract class AggregateProfile<TDto, TAggregateRoot> : Profile where TAggregateRoot : IAggregateRoot where TDto : IDto 
    {
        protected AggregateProfile()
        {
            //Configure base mappings
            CreateMap<TAggregateRoot, TDto>();
            CreateMap<TDto, TAggregateRoot>().ForAllMembers(GetConfiguredOptions());
        }

        protected static Action<IMemberConfigurationExpression<TDto, TAggregateRoot, object>> GetConfiguredOptions()
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