using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Factories
{
    public class InventionTypeFactory : Factory<InventionTypeDto, InventionType>,  IInventionTypeFactory
    {
        public InventionTypeFactory(IMapper mapper) : base(mapper)
        {
        }

        public override InventionType Create(InventionTypeDto dto)
        {
            return new InventionType(
                dto.Code,
                dto.Name,
                dto.Description
            );
        }
    }
}