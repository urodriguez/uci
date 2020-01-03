using Application.Contracts.Factories;
using Application.Dtos;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Factories
{
    public class ProductFactory : Factory<ProductDto, Product>, IProductFactory
    {
        public ProductFactory(IMapper mapper) : base(mapper)
        {
        }

        //public new Product Adapt(ProductDto productDto, Product existingProduct = default(Product))
        //{
        //    var product = existingProduct != null ? _mapper.Map<ProductDto, Product>(productDto, existingProduct) : _mapper.Map<ProductDto, Product>(productDto);

        //    //TODO: check how build the aggregate (maybe injecting the categoryRepository?)
        //    //product.Category = new Category { Code = productDto.CategoryCode };

        //    return product;
        //}
    }
}