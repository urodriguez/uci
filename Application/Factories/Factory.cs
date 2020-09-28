using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Contracts.Aggregates;
#pragma warning disable 1998

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

        public virtual async Task<TDto> CreateAsync(TAggregateRoot aggregate)
        {
            return _mapper.Map<TAggregateRoot, TDto>(aggregate);
        }

        public async Task<IEnumerable<TDto>> CreateFromRange(IEnumerable<TAggregateRoot> aggregates)
        {
            var dtos = new List<TDto>();
            foreach (var aggregate in aggregates)
            {
                dtos.Add(await CreateAsync(aggregate));
            }
            return dtos;
        }

        /// <summary>
        /// Creates new aggregate based on Dto information
        /// </summary>
        public abstract TAggregateRoot Create(TDto dto);
    }
}