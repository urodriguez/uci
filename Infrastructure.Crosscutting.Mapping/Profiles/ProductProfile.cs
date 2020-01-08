using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public class ProductProfile : AggregateProfile<ProductDto, Product>
    {
        public ProductProfile()
        {
            //Add more custom mappings
        }
    }
}