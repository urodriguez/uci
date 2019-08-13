using System.Collections.Generic;
using Application.Contracts.Adapters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.Adapters
{
    public class ProductTypeAdapter : IProductTypeAdapter
    {
        public ProductTypeDto Adapt(ProductType aggregate)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProductTypeDto> AdaptRange(IEnumerable<ProductType> aggregates)
        {
            throw new System.NotImplementedException();
        }

        public ProductType Adapt(ProductTypeDto dto)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProductType> AdaptRange(IEnumerable<ProductTypeDto> dtos)
        {
            throw new System.NotImplementedException();
        }
    }
}