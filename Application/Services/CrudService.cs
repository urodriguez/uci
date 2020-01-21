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
using Domain.Enums;

namespace Application.Services
{
    public abstract class CrudService<TDto, TAggregateRoot> : ApplicationService, ICrudService<TDto> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        private readonly IRepository<TAggregateRoot> _repository;
        protected readonly IFactory<TDto, TAggregateRoot> _factory;
        protected readonly IAuditService _auditService;
        private readonly IBusinessValidator<TDto> _businessValidator;
        //private readonly ILoggerService _loggerService;

        protected CrudService(
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
        }

        public IEnumerable<TDto> GetAll()
        {
            CheckAuthorization();

            var aggregates = _repository.Get();

            var dtos = _factory.CreateFromRange(aggregates);

            return dtos;
        }

        public TDto GetById(Guid id)
        {
            CheckAuthorization();

            var aggregate = _repository.GetById(id);

            if (aggregate == null) throw new ObjectNotFoundException();

            return _factory.Create(aggregate);
        }

        public Guid Create(TDto dto)
        {
            CheckAuthorization();

            _businessValidator.Validate(dto);

            var aggregate = _factory.Create(dto);

            _repository.Add(aggregate);

            _auditService.Audit(aggregate, AuditAction.Create);

            return aggregate.Id;
        }

        public void Update(Guid id, TDto dto)
        {
            CheckAuthorization();

            _businessValidator.Validate(dto, id);

            var aggregate = _repository.GetById(id);

            if (aggregate == null) throw new ObjectNotFoundException();

            var aggregateUpdated = _factory.CreateFromExisting(dto, aggregate);
            
            _repository.Update(aggregateUpdated);

            _auditService.Audit(aggregateUpdated, AuditAction.Update);
        }

        public void Delete(Guid id)
        {
            CheckAuthorization();

            var aggregate = _repository.GetById(id);

            if (aggregate == null) throw new ObjectNotFoundException();

            _repository.Delete(aggregate);

            _auditService.Audit(aggregate, AuditAction.Delete);
        }
    }
}
