using System.Collections.Generic;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
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
            IProductFactory factory, 
            IAuditService auditService,
            IProductBusinessValidator productBusinessValidator
        ) : base(
            productRepository, 
            factory, 
            auditService, 
            productBusinessValidator
        )
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductDto> GetCheapest(decimal maxPrice)
        {
            var cheapestProducts = _productRepository.Get(new CheapestIAPG(maxPrice));
            return _factory.CreateFromRange(cheapestProducts);
        }
    }
}
