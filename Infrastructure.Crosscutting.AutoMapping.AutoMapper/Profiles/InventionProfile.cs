using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.AutoMapping.AutoMapper.Profiles
{
    public class InventionProfile : AggregateProfile<InventionDto, Invention>
    {
        public InventionProfile()
        {
            //Add more custom mappings
        }
    }
}