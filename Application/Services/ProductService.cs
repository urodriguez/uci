using System.Collections.Generic;
using Application.ApplicationResults;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;

namespace Application.Services
{
    public class ProductService : CrudService<ProductDto, Product>, IProductService
    {
        private readonly IProductPredicateFactory _productPredicateFactory;

        public ProductService(
            IRoleService roleService,
            IProductFactory factory, 
            IAuditService auditService,
            IProductBusinessValidator productBusinessValidator, 
            IProductPredicateFactory productPredicateFactory,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            ILogService logService,
            IAppSettingsService appSettingsService
        ) : base(
            roleService,
            factory, 
            auditService, 
            productBusinessValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService
        )
        {
            _productPredicateFactory = productPredicateFactory;
        }

        public IApplicationResult GetCheapest(decimal maxPrice)
        {
            return Execute(() =>
            {
                var byCheapest = _productPredicateFactory.CreateByCheapest(maxPrice);
                var cheapestProducts = _unitOfWork.Products.Get(byCheapest);

                var cheapestProductsDto = _factory.CreateFromRange(cheapestProducts);

                return new OkApplicationResult<IEnumerable<ProductDto>>
                {
                    Data = cheapestProductsDto
                };
            });
        }
    }
}
