using System;
using System.Collections.Generic;
using System.Data;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Auditing;

namespace Application.Services
{
    public abstract class CrudService<TDto, TAggregateRoot> : ICrudService<TDto> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        private readonly IRepository<TAggregateRoot> _repository;
        protected readonly IAdapter<TDto, TAggregateRoot> _adapter;
        protected readonly IAuditService _auditService;
        //private readonly IBusinessValidator _businessValidator;
        //private readonly ILoggerService _loggerService;

        protected CrudService(IRepository<TAggregateRoot> repository, IAdapter<TDto, TAggregateRoot> adapter, IAuditService auditService)
        {
            _repository = repository;
            _adapter = adapter;
            _auditService = auditService;
        }

        public IEnumerable<TDto> GetAll()
        {
            var aggregates = _repository.GetAll();

            var dtos = _adapter.AdaptRange(aggregates);

            return dtos;
        }

        public TDto GetById(Guid id)
        {
            var aggregate = _repository.GetById(id);

            if (aggregate == null) throw new ObjectNotFoundException();

            return _adapter.Adapt(aggregate);
        }

        public Guid Create(TDto dto)
        {
            //_businessValidator.Validate(aggregate) -> validate data from UI or external service (example: product name, price positive, valid category)

            var aggregate = _adapter.Adapt(dto);
            
            _repository.Add(aggregate);

            _auditService.Audit(aggregate, AuditAction.Create);

            return aggregate.Id;
        }

        public void Update(Guid id, TDto dto)
        {
            var oldAggregate = _repository.GetById(id);

            var aggregate = _repository.GetById(id);

            if (aggregate == null) throw new ObjectNotFoundException();

            _adapter.Adapt(dto, aggregate);
            aggregate.Id = id;

            //_businessValidator.Validate(aggregate)
            _repository.Update(aggregate);

            _auditService.Audit(aggregate, AuditAction.Update, oldAggregate);
        }

        public void Delete(Guid id)
        {
            var aggregate = _repository.GetById(id);

            if (aggregate == null) throw new ObjectNotFoundException();

            _repository.Delete(aggregate);

            _auditService.Audit(aggregate, AuditAction.Delete);
        }
    }
}
