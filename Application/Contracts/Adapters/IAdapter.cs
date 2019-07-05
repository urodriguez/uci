using System.Collections;
using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.Adapters
{
  public interface IAdapter
  {
    IDto Adapt(IAggregate aggregate);
    IEnumerable<IDto> AdaptBulk(IEnumerable<IAggregate> aggregates);
    IAggregate Adapt(IDto dto);
    IEnumerable<IAggregate> AdaptBulk(IEnumerable<IDto> dtos);
  }
}
