using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class ProductTypeService : CrudService<ProductTypeDto, ProductType>, IProductTypeService
    {
        public ProductTypeService(
            IProductTypeRepository repository, 
            IProductTypeFactory factory, 
            IAuditService auditService,
            IProductTypeBusinessValidator productTypeBusinessValidator,
            ITokenService tokenService
        ) : base(
            repository,
            factory, 
            auditService, 
            productTypeBusinessValidator,
            tokenService
        )
        {
        }
    }
}