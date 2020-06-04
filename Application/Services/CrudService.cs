using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Newtonsoft.Json;

namespace Application.Services
{
    public abstract class CrudService<TDto, TAggregateRoot> : ApplicationService, ICrudService<TDto> 
        where TAggregateRoot : class, IAggregateRoot 
        where TDto : IDto
    {
        protected readonly IRoleService _roleService;
        protected readonly IFactory<TDto, TAggregateRoot> _factory;
        protected readonly IAuditService _auditService;
        protected readonly IBusinessValidator<TDto> _businessValidator;
        protected readonly IAppSettingsService _appSettingsService;

        protected CrudService(
            IRoleService roleService,
            IFactory<TDto, TAggregateRoot> factory, 
            IAuditService auditService, 
            IBusinessValidator<TDto> businessValidator, 
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            ILogService logService,
            IAppSettingsService appSettingsService,
            IInventAppContext inventAppContext
        ) : base (tokenService, unitOfWork, logService, inventAppContext)
        {
            _factory = factory;
            _auditService = auditService;
            _businessValidator = businessValidator;
            _roleService = roleService;
            _appSettingsService = appSettingsService;
        }

        public virtual async Task<IApplicationResult> GetAllAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var aggregates = await _unitOfWork.GetRepository<TAggregateRoot>().GetAsync();

                var dtos = _factory.CreateFromRange(aggregates);

                return new OkApplicationResult<IEnumerable<TDto>>
                {
                    Data = dtos
                };
            });
        }

        public virtual async Task<IApplicationResult> GetByIdAsync(Guid id)
        {
            return await ExecuteAsync(async () =>
            {
                var aggregate = await _unitOfWork.GetRepository<TAggregateRoot>().GetByIdAsync(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                var dto = _factory.Create(aggregate);

                return new OkApplicationResult<TDto>
                {
                    Data = dto
                };
            });
        }

        public virtual async Task<IApplicationResult> CreateAsync(TDto dto)
        {
            return await ExecuteAsync(async () =>
            {
                var isAdmin = await _roleService.IsAdmin(_inventAppContext.UserName);
                if (!isAdmin) 
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserName}'");

                await _businessValidator.ValidateAsync(dto);

                var aggregate = _factory.Create(dto);

                await _unitOfWork.GetRepository<TAggregateRoot>().AddAsync(aggregate);

                _auditService.AuditAsync(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = _inventAppContext.UserName,
                    EntityId = aggregate.Id.ToString(),
                    EntityName = aggregate.GetType().Name,
                    Entity = JsonConvert.SerializeObject(aggregate),
                    Action = AuditAction.Create
                });

                return new OkApplicationResult<Guid>
                {
                    Data = aggregate.Id
                };
            });
        }

        public virtual async Task<IApplicationResult> UpdateAsync(Guid id, TDto dto)
        {
            return await ExecuteAsync(async () =>
            {
                var isAdmin = await _roleService.IsAdmin(_inventAppContext.UserName);
                if (!isAdmin)
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserName}'");

                await _businessValidator.ValidateAsync(dto, id);

                var aggregate = await _unitOfWork.GetRepository<TAggregateRoot>().GetByIdAsync(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                var aggregateUpdated = _factory.CreateFromExisting(dto, aggregate);

                await _unitOfWork.GetRepository<TAggregateRoot>().UpdateAsync(aggregateUpdated);

                _auditService.AuditAsync(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = _inventAppContext.UserName,
                    EntityId = aggregate.Id.ToString(),
                    EntityName = aggregate.GetType().Name,
                    Entity = JsonConvert.SerializeObject(aggregateUpdated),
                    Action = AuditAction.Update
                });

                return new OkEmptyResult();
            });
        }

        public virtual async Task<IApplicationResult> DeleteAsync(Guid id)
        {
            return await ExecuteAsync(async () =>
            {
                var isAdmin = await _roleService.IsAdmin(_inventAppContext.UserName);
                if (!isAdmin)
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserName}'");

                var aggregate = await _unitOfWork.GetRepository<TAggregateRoot>().GetByIdAsync(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                await _unitOfWork.GetRepository<TAggregateRoot>().DeleteAsync(aggregate);

                _auditService.AuditAsync(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = _inventAppContext.UserName,
                    EntityId = aggregate.Id.ToString(),
                    EntityName = aggregate.GetType().Name,
                    Entity = JsonConvert.SerializeObject(aggregate),
                    Action = AuditAction.Delete
                });

                return new OkEmptyResult();
            });
        }
    }
}
