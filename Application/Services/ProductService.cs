using System.Collections.Generic;
using Application.ApplicationResults;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;

namespace Application.Services
{
    public class ProductService : CrudService<ProductDto, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPredicateFactory _productPredicateFactory;

        public ProductService(
            IRoleService roleService,
            IProductRepository productRepository, 
            IProductFactory factory, 
            IAuditService auditService,
            IProductBusinessValidator productBusinessValidator, 
            IProductPredicateFactory productPredicateFactory,
            ITokenService tokenService,
            ILogService logService
        ) : base(
            roleService,
            productRepository, 
            factory, 
            auditService, 
            productBusinessValidator,
            tokenService,
            logService
        )
        {
            _productRepository = productRepository;
            _productPredicateFactory = productPredicateFactory;
        }

        public IApplicationResult GetCheapest(decimal maxPrice)
        {
            return Execute(() =>
            {
                var byCheapest = _productPredicateFactory.CreateByCheapest(maxPrice);
                var cheapestProducts = _productRepository.Get(byCheapest);

                var cheapestProductsDto = _factory.CreateFromRange(cheapestProducts);

                return new ApplicationResult<IEnumerable<ProductDto>>
                {
                    Status = ApplicationStatus.Ok,
                    Message = "Cheapest products found",
                    Data = cheapestProductsDto
                };
            });
        }
    }
}
