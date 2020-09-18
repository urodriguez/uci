using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Factories
{
    public class InventionCategoryFactory : Factory<InventionCategoryDto, InventionCategory>,  IInventionCategoryFactory
    {
        public InventionCategoryFactory(IMapper mapper) : base(mapper)
        {
        }

        public override InventionCategory Create(InventionCategoryDto dto)
        {
            return new InventionCategory(
                dto.Code,
                dto.Name,
                dto.Description
            );
        }
    }
}