using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.Adapters
{
  public interface IAdapter<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
    IDto Adapt(TAggregateRoot aggregate);
    IEnumerable<IDto> AdaptRange(IEnumerable<TAggregateRoot> aggregates);
    TAggregateRoot Adapt(IDto dto);
    IEnumerable<TAggregateRoot> AdaptRange(IEnumerable<IDto> dtos);
  }
}
