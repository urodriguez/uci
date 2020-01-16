using System.Collections.Generic;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;

namespace Application.Services
{
    public class ProductService : CrudService<ProductDto, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPredicateFactory _productPredicateFactory;

        public ProductService(
            IProductRepository productRepository, 
            IProductFactory factory, 
            IAuditService auditService,
            IProductBusinessValidator productBusinessValidator, 
            IProductPredicateFactory productPredicateFactory
        ) : base(
            productRepository, 
            factory, 
            auditService, 
            productBusinessValidator
        )
        {
            _productRepository = productRepository;
            _productPredicateFactory = productPredicateFactory;
        }

        public IEnumerable<ProductDto> GetCheapest(decimal maxPrice)
        {
            var byCheapest = _productPredicateFactory.CreateByCheapest(maxPrice);
            var cheapestProducts = _productRepository.Get(byCheapest);

            return _factory.CreateFromRange(cheapestProducts);
        }
    }
}
