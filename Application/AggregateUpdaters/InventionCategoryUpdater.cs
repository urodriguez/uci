using Application.Contracts.AggregateUpdaters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.AggregateUpdaters
{
    public class InventionCategoryUpdater : IInventionCategoryUpdater
    {
        public void Update(InventionCategory inventionCategory, InventionCategoryDto dto)
        {
            inventionCategory.SetCode(dto.Code);
            inventionCategory.SetName(dto.Name);
            inventionCategory.SetDescription(dto.Description);
        }
    }
}