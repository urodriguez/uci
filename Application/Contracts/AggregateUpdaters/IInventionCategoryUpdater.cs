using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.AggregateUpdaters
{
    public interface IInventionCategoryUpdater : IAggregateUpdater<InventionCategoryDto, InventionCategory>
    {
        
    }
}