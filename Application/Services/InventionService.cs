using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Application.ApplicationResults;
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
using Application.Infrastructure.Auditing;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Newtonsoft.Json;

namespace Application.Services
{
    public class InventionService : CrudService<InventionDto, Invention>, IInventionService
    {
        private readonly IInventionPredicateFactory _inventionPredicateFactory;

        public InventionService(
            IRoleService roleService,
            IInventionFactory factory,
            IInventionUpdater updater,
            IAuditService auditService,
            IInventionDuplicateValidator inventionDuplicateValidator, 
            IInventionPredicateFactory inventionPredicateFactory,
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
            inventionDuplicateValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService,
            inventAppContext
        )
        {
            _inventionPredicateFactory = inventionPredicateFactory;
        }

        public async Task<IApplicationResult> GetCheapestAsync(decimal maxPrice)
        {
            return await ExecuteAsync(async () =>
            {
                var byCheapest = _inventionPredicateFactory.CreateByCheapest(maxPrice);
                var cheapestInventions = await _unitOfWork.Inventions.GetAsync(byCheapest);

                var cheapestInventionsDto = await _factory.CreateFromRange(cheapestInventions);

                return new OkApplicationResult<IEnumerable<InventionDto>>
                {
                    Data = cheapestInventionsDto
                };
            });
        }        
        
        public async Task<IApplicationResult> UpdateStateAsync(Guid id, InventionStateDto dto)
        {
            return await ExecuteAsync(async () =>
            {
                var isAdmin = await _roleService.IsAdminAsync(_inventAppContext.UserEmail);
                if (!isAdmin)
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserEmail}'");

                var aggregate = await _unitOfWork.Inventions.GetByIdAsync(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                aggregate.Enable = dto.Enable;

                await _unitOfWork.Inventions.UpdateAsync(aggregate);

                _auditService.AuditAsync(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = _inventAppContext.UserEmail,
                    EntityId = aggregate.Id.ToString(),
                    EntityName = aggregate.GetType().Name,
                    Entity = JsonConvert.SerializeObject(aggregate),
                    Action = AuditAction.Update
                });

                return new OkEmptyResult();
            });
        }
    }
}
