using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.AggregateUpdaters
{
    public interface IInventionTypeUpdater : IAggregateUpdater<InventionTypeDto, InventionType>
    {
        
    }
}