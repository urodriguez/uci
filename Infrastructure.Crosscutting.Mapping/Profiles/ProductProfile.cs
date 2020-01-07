using Application.Dtos;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public class ProductProfile : AggregateProfile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>().ForAllMembers(GetConfiguredOptions<ProductDto, Product>());
        }
    }
}