using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;

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
            ILogService logService,
            IAppSettingsService appSettingsService
        ) : base(
            roleService,
            repository,
            factory, 
            auditService, 
            productTypeBusinessValidator,
            tokenService,
            logService,
            appSettingsService
        )
        {
        }
    }
}