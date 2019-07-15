using System;
using System.Collections.Generic;
using Application.Contracts;
using Application.Contracts.Adapters;
using Application.Dtos;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.Services
{
    public class CrudService<TAggregateRoot> : ICrudService<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        private readonly IRepository<TAggregateRoot> _repository;
        private readonly IAdapter<TAggregateRoot> _adapter;
        //private readonly IBusinessValidator _businessValidator;
        //private readonly ILoggerService _loggerService;

      public CrudService(IRepository<TAggregateRoot> repository, IAdapter<TAggregateRoot> adapter)
        {
            _repository = repository;
            _adapter = adapter;
        }

        public IEnumerable<IDto> GetAll()
        {
            var aggregates = _repository.GetAll();
            var dtos = _adapter.AdaptRange(aggregates);
            return dtos;
        }

        public IDto GetById(Guid id)
        {
            var aggregate = _repository.GetById(id);
            var dto = _adapter.Adapt(aggregate);
            return dto;
        }

        public Guid Create(IDto dto)
        {
            var aggregate = _adapter.Adapt(dto);
            //_businessValidator.Validate(aggregate)
            _repository.Add(aggregate);
            return aggregate.Id;
        }

        public void Update(IDto dto)
        {
            var aggregate = _adapter.Adapt(dto);
            //_businessValidator.Validate(aggregate)
            _repository.Update(aggregate);
        }

        public void Delete(Guid id)
        {
            _repository.Remove(id);
        }

        public void DeleteRange(IEnumerable<Guid> ids)
        {
            _repository.RemoveRange(ids);
        }
    }
}
