using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Adapters;
using Application.Dtos;
using AutoMapper;
using Domain.Contracts.Aggregates;

namespace Application.Adapters
{
    public abstract class Adapter<TDto, TAggregateRoot> : IAdapter<TDto, TAggregateRoot> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        protected readonly IMapper _mapper;

        protected Adapter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDto Adapt(TAggregateRoot aggregate)
        {
            return _mapper.Map<TAggregateRoot, TDto>(aggregate);
        }

        public IEnumerable<TDto> AdaptRange(IEnumerable<TAggregateRoot> aggregates)
        {
            return aggregates.Select(Adapt);
        }

        //Adapt existing element or create one if none one is provided
        public TAggregateRoot Adapt(TDto dto, TAggregateRoot existingAggregate = default(TAggregateRoot))
        {
            return existingAggregate != null ? _mapper.Map<TDto, TAggregateRoot>(dto, existingAggregate) : _mapper.Map<TDto, TAggregateRoot>(dto);
        }
    }
}