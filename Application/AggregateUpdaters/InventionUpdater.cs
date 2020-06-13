using Application.Contracts.AggregateUpdaters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.AggregateUpdaters
{
    public class InventionUpdater : IInventionUpdater
    {
        public void Update(Invention invention, InventionDto dto)
        {
            invention.SetCode(dto.Code);
            invention.SetName(dto.Name);
            invention.SetCategory(dto.Category);
            invention.SetPrice(dto.Price);
        }
    }
}