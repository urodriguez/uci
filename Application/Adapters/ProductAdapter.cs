using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Adapters;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Aggregates;

namespace Application.Adapters
{
    public class ProductAdapter : IProductAdapter
    {
        public IDto Adapt(Product aggregate)
        {
            return new ProductDto
            {
                Id = aggregate.Id,
                Category = aggregate.Category,
                Name = aggregate.Name,
                Price = aggregate.Price
            };
        }

        public IEnumerable<IDto> AdaptRange(IEnumerable<Product> aggregates)
        {
            return aggregates.Select(Adapt);
        }

        public Product Adapt(IDto dto)
        {
            var productDto = (ProductDto)dto;
            return new Product
            {
                Id = productDto.Id,
                Category = productDto.Category,
                Name = productDto.Name,
                Price = productDto.Price
            };
        }

        public IEnumerable<Product> AdaptRange(IEnumerable<IDto> dtos)
        {
            return dtos.Select(Adapt);
        }
    }
}