using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.Adapters
{
    public interface IAdapter<TDto, TAggregateRoot> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        TDto Adapt(TAggregateRoot aggregate);
        IEnumerable<TDto> AdaptRange(IEnumerable<TAggregateRoot> aggregates);
        TAggregateRoot Adapt(TDto dto);
        IEnumerable<TAggregateRoot> AdaptRange(IEnumerable<TDto> dtos);
    }
}
