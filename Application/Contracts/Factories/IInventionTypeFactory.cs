using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IInventionTypeFactory : IFactory<InventionTypeDto, InventionType>
    {
        
    }
}