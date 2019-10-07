using System.Collections.Generic;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.Services
{
    public class ProductService : CrudService<ProductDto, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository, IProductAdapter adapter, IAuditService auditService) : base(productRepository, adapter, auditService)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductDto> GetCheapest(decimal maxPrice)
        {
            var cheapestProducts = _productRepository.GetCheapest(maxPrice);
            return _adapter.AdaptRange(cheapestProducts);
        }
    }
}
