using Application.Contracts.AggregateUpdaters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.AggregateUpdaters
{
    public class InventionUpdater : IInventionUpdater
    {
        public void Update(Invention inventionCategory, InventionDto dto)
        {
            inventionCategory.SetCode(dto.Code);
            inventionCategory.SetName(dto.Name);
            inventionCategory.Description = dto.Description;
            inventionCategory.CategoryId =  dto.CategoryId;
            inventionCategory.SetPrice(dto.Price);
            inventionCategory.Enable = dto.Enable;
        }
    }
}