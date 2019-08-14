using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Infrastructure.Crosscutting.Mapping.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}