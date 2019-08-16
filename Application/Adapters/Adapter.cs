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

        public TAggregateRoot Adapt(TDto dto)
        {
            return _mapper.Map<TDto, TAggregateRoot>(dto);
        }

        public IEnumerable<TAggregateRoot> AdaptRange(IEnumerable<TDto> dtos)
        {
            return dtos.Select(Adapt);
        }
    }
}