using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IFactory<TDto, TAggregateRoot> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        TDto Create(TAggregateRoot aggregate);
        IEnumerable<TDto> CreateFromRange(IEnumerable<TAggregateRoot> aggregates);
        TAggregateRoot Create(TDto dto);
        TAggregateRoot CreateFromExisting(TDto dto, TAggregateRoot aggregate);
    }
}
