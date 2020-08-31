using Application.Contracts;
using Application.Contracts.AggregateUpdaters;
using Application.Contracts.DuplicateValidators;
using Application.Contracts.Factories;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Auditing;
using Application.Contracts.Infrastructure.Authentication;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Services;

namespace Application.Services
{
    public class InventionTypeService : CrudService<InventionTypeDto, InventionType>, IInventionTypeService
    {
        public InventionTypeService(
            IRoleService roleService,
            IInventionTypeFactory factory, 
            IInventionTypeUpdater updater, 
            IAuditService auditService,
            IInventionTypeDuplicateValidator inventionTypeDuplicateValidator,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            ILogService logService,
            IAppSettingsService appSettingsService,
            IInventAppContext inventAppContext
        ) : base(
            roleService,
            factory,
            updater,
            auditService, 
            inventionTypeDuplicateValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService,
            inventAppContext
        )
        {
        }
    }
}