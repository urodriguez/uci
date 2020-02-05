using System;
using System.Collections.Generic;
using System.Data;
using Application.ApplicationResults;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Services;
using Domain.Enums;

namespace Application.Services
{
    public abstract class CrudService<TDto, TAggregateRoot> : ApplicationService, ICrudService<TDto> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        private readonly IRoleService _roleService;
        private readonly IRepository<TAggregateRoot> _repository;
        protected readonly IFactory<TDto, TAggregateRoot> _factory;
        protected readonly IAuditService _auditService;
        private readonly IBusinessValidator<TDto> _businessValidator;

        protected CrudService(
            IRoleService roleService,
            IRepository<TAggregateRoot> repository, 
            IFactory<TDto, TAggregateRoot> factory, 
            IAuditService auditService, 
            IBusinessValidator<TDto> businessValidator, 
            ITokenService tokenService,
            ILogService logService
        ) : base (tokenService, logService)
        {
            _repository = repository;
            _factory = factory;
            _auditService = auditService;
            _businessValidator = businessValidator;
            _roleService = roleService;
        }

        public IApplicationResult GetAll()
        {
            return Execute(() =>
            {
                var aggregates = _repository.Get();

                var dtos = _factory.CreateFromRange(aggregates);

                return new ApplicationResult<IEnumerable<TDto>>
                {
                    Status = ApplicationStatus.Ok,
                    Message = "Entries found",
                    Data = dtos
                };
            });
        }

        public IApplicationResult GetById(Guid id)
        {
            return Execute(() =>
            {
                var aggregate = _repository.GetById(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                var dto = _factory.Create(aggregate);

                return new ApplicationResult<TDto>
                {
                    Status = ApplicationStatus.Ok,
                    Message = "Entry found",
                    Data = dto
                };
            });
        }

        public IApplicationResult Create(TDto dto)
        {
            return Execute(() =>
            {
                if (!_roleService.IsAdmin(InventAppContext.UserName)) throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{InventAppContext.UserName}'");

                _businessValidator.Validate(dto);

                var aggregate = _factory.Create(dto);

                _repository.Add(aggregate);

                _auditService.Audit(aggregate, AuditAction.Create, InventAppContext.UserName);

                return new ApplicationResult<Guid>
                {
                    Status = ApplicationStatus.Ok,
                    Message = "Entry has been created",
                    Data = aggregate.Id
                };
            });
        }

        public IApplicationResult Update(Guid id, TDto dto)
        {
            return Execute(() =>
            {
                if (!_roleService.IsAdmin(InventAppContext.UserName)) throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{InventAppContext.UserName}'");

                _businessValidator.Validate(dto, id);

                var aggregate = _repository.GetById(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                var aggregateUpdated = _factory.CreateFromExisting(dto, aggregate);

                _repository.Update(aggregateUpdated);

                _auditService.Audit(aggregateUpdated, AuditAction.Update, InventAppContext.UserName);

                return new EmptyResult
                {
                    Status = ApplicationStatus.Ok,
                    Message = $"Entry with Id={aggregate.Id} has been updated"
                };
            });
        }

        public IApplicationResult Delete(Guid id)
        {
            return Execute(() =>
            {
                if (!_roleService.IsAdmin(InventAppContext.UserName)) throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{InventAppContext.UserName}'");

                var aggregate = _repository.GetById(id);

                if (aggregate == null) throw new ObjectNotFoundException($"Entry with Id={id} not found");

                _repository.Delete(aggregate);

                _auditService.Audit(aggregate, AuditAction.Delete, InventAppContext.UserName);

                return new EmptyResult
                {
                    Status = ApplicationStatus.Ok,
                    Message = $"Entry with Id={aggregate.Id} has been deleted"
                };
            });
        }
    }
}
