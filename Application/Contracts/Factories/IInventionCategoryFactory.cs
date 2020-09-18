using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IInventionCategoryFactory : IFactory<InventionCategoryDto, InventionCategory>
    {
        
    }
}