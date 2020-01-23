using System;
using System.Collections.Generic;
using System.Data;
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
        //private readonly ILoggerService _loggerService;

        protected CrudService(
            IRoleService roleService,
            IRepository<TAggregateRoot> repository, 
            IFactory<TDto, TAggregateRoot> factory, 
            IAuditService auditService, 
            IBusinessValidator<TDto> businessValidator, 
            ITokenService tokenService
        ) : base (tokenService)
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
                    Status = 1,
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

                if (aggregate == null) throw new ObjectNotFoundException();

                var dto = _factory.Create(aggregate);

                return new ApplicationResult<TDto>
                {
                    Status = 1,
                    Message = "Entry found",
                    Data = dto
                };
            });
        }

        public IApplicationResult Create(TDto dto)
        {
            return Execute(() =>
            {
                if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();

                _businessValidator.Validate(dto);

                var aggregate = _factory.Create(dto);

                _repository.Add(aggregate);

                _auditService.Audit(aggregate, AuditAction.Create);

                return new ApplicationResult<Guid>
                {
                    Status = 1,
                    Message = "New entry has been created with it respective Id",
                    Data = aggregate.Id
                };
            });
        }

        public IApplicationResult Update(Guid id, TDto dto)
        {
            return Execute(() =>
            {
                if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();

                _businessValidator.Validate(dto, id);

                var aggregate = _repository.GetById(id);

                if (aggregate == null) throw new ObjectNotFoundException();

                var aggregateUpdated = _factory.CreateFromExisting(dto, aggregate);

                _repository.Update(aggregateUpdated);

                _auditService.Audit(aggregateUpdated, AuditAction.Update);

                return new EmptyResult
                {
                    Status = 1,
                    Message = $"Entry with Id={aggregate.Id} has been updated"
                };
            });
        }

        public IApplicationResult Delete(Guid id)
        {
            return Execute(() =>
            {
                if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();

                var aggregate = _repository.GetById(id);

                if (aggregate == null) throw new ObjectNotFoundException();

                _repository.Delete(aggregate);

                _auditService.Audit(aggregate, AuditAction.Delete);

                return new EmptyResult
                {
                    Status = 1,
                    Message = $"Entry with Id={aggregate.Id} has been deleted"
                };
            });
        }
    }
}
