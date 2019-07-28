using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Adapters;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.Adapters
{
    public class ProductAdapter : IProductAdapter
    {
        public ProductDto Adapt(Product aggregate)
        {
            return new ProductDto
            {
                Id = aggregate.Id,
                Category = aggregate.Category,
                Name = aggregate.Name,
                Price = aggregate.Price
            };
        }

        public IEnumerable<ProductDto> AdaptRange(IEnumerable<Product> aggregates)
        {
            return aggregates.Select(Adapt);
        }

        public Product Adapt(ProductDto dto)
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

        public IEnumerable<Product> AdaptRange(IEnumerable<ProductDto> dtos)
        {
            return dtos.Select(Adapt);
        }
    }
}