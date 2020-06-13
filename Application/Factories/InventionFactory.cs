using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Factories
{
    public class InventionFactory : Factory<InventionDto, Invention>, IInventionFactory
    {
        public InventionFactory(IMapper mapper) : base(mapper)
        {
        }

        public override Invention Create(InventionDto dto)
        {
            return new Invention(
                dto.Code,
                dto.Name,
                dto.Category,
                dto.Price
            );
        }
    }
}