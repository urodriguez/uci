using System.Collections.Generic;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.BusinessValidators;
using Domain.Contracts.Repositories;
using Domain.Predicates;
using Infrastructure.Crosscutting.Auditing;

namespace Application.Services
{
    public class ProductService : CrudService<ProductDto, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(
            IProductRepository productRepository, 
            IProductAdapter adapter, 
            IAuditService auditService,
            IProductBusinessValidator productBusinessValidator
        ) : base(
            productRepository, 
            adapter, 
            auditService, 
            productBusinessValidator
        )
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductDto> GetCheapest(decimal maxPrice)
        {
            var cheapestProducts = _productRepository.Get(new CheapestIAPG(maxPrice));
            return _adapter.AdaptRange(cheapestProducts);
        }
    }
}
