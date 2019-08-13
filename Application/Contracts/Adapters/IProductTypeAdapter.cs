using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Adapters
{
    public interface IProductTypeAdapter : IAdapter<ProductTypeDto, ProductType>
    {
        
    }
}