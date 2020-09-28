using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IFactory<TDto, TAggregateRoot> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        Task<TDto> CreateAsync(TAggregateRoot aggregate);
        Task<IEnumerable<TDto>> CreateFromRange(IEnumerable<TAggregateRoot> aggregates);
        TAggregateRoot Create(TDto dto);
    }
}
