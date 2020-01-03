using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Factories
{
    public class ProductTypeFactory : Factory<ProductTypeDto, ProductType>,  IProductTypeFactory
    {
        public ProductTypeFactory(IMapper mapper) : base(mapper)
        {
        }
    }
}