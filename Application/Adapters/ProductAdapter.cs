using Application.Contracts.Adapters;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Adapters
{
    public class ProductAdapter : Adapter<ProductDto, Product>, IProductAdapter
    {
        public ProductAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}