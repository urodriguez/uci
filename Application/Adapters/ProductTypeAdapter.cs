using Application.Contracts.Adapters;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Adapters
{
    public class ProductTypeAdapter : Adapter<ProductTypeDto, ProductType>,  IProductTypeAdapter
    {
        public ProductTypeAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}