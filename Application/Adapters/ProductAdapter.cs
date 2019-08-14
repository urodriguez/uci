using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Adapters;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Adapters
{
    public class ProductAdapter : IProductAdapter
    {
        private readonly IMapper _mapper;
        public ProductAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ProductDto Adapt(Product aggregate)
        {
            //return new ProductDto
            //{
            //    Id = aggregate.Id,
            //    Category = aggregate.Category,
            //    Name = aggregate.Name,
            //    Price = aggregate.Price
            //};
            var p = _mapper.Map<Product, ProductDto>(aggregate);
            return p;
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