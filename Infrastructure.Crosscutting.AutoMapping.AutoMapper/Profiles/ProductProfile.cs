using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.AutoMapping.AutoMapper.Profiles
{
    public class ProductProfile : AggregateProfile<ProductDto, Product>
    {
        public ProductProfile()
        {
            //Add more custom mappings
        }
    }
}