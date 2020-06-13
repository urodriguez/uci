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
        }
    }
}