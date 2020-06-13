using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.AggregateUpdaters
{
    public interface IInventionUpdater : IAggregateUpdater<InventionDto, Invention>
    {
    }
}