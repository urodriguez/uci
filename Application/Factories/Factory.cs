using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Contracts.Aggregates;

namespace Application.Factories
{
    public abstract class Factory<TDto, TAggregateRoot> : IFactory<TDto, TAggregateRoot> 
        where TAggregateRoot : class, IAggregateRoot 
        where TDto : IDto
    {
        protected readonly IMapper _mapper;

        protected Factory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDto Create(TAggregateRoot aggregate)
        {
            return _mapper.Map<TAggregateRoot, TDto>(aggregate);
        }

        public IEnumerable<TDto> CreateFromRange(IEnumerable<TAggregateRoot> aggregates)
        {
            return aggregates.Select(Create);
        }

        /// <summary>
        /// Creates new aggregate based on Dto information
        /// </summary>
        public abstract TAggregateRoot Create(TDto dto);

        /// <summary>
        /// Creates new aggregate based on Dto information and existing aggregate
        /// </summary>
        public TAggregateRoot CreateFromExisting(TDto dto, TAggregateRoot existingAggregate)
        {
            var aggregate = _mapper.Map<TDto, TAggregateRoot>(dto, existingAggregate);
            return aggregate;
        }

    }
}