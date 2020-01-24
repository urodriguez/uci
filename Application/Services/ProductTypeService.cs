using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Services;

namespace Application.Services
{
    public class ProductTypeService : CrudService<ProductTypeDto, ProductType>, IProductTypeService
    {
        public ProductTypeService(
            IRoleService roleService,
            IProductTypeRepository repository, 
            IProductTypeFactory factory, 
            IAuditService auditService,
            IProductTypeBusinessValidator productTypeBusinessValidator,
            ITokenService tokenService,
            ILogService logService
        ) : base(
            roleService,
            repository,
            factory, 
            auditService, 
            productTypeBusinessValidator,
            tokenService,
            logService
        )
        {
        }
    }
}