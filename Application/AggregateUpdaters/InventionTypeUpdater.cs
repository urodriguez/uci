using Application.Contracts.AggregateUpdaters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.AggregateUpdaters
{
    public class InventionTypeUpdater : IInventionTypeUpdater
    {
        public void Update(InventionType inventionType, InventionTypeDto dto)
        {
            inventionType.SetCode(dto.Code);
            inventionType.SetName(dto.Name);
            inventionType.SetDescription(dto.Description);
        }
    }
}