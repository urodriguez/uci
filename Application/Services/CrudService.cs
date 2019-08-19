using System;
using System.Collections.Generic;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.Services
{
    public class CrudService<TDto, TAggregateRoot> : ICrudService<TDto> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        private readonly IRepository<TAggregateRoot> _repository;
        private readonly IAdapter<TDto, TAggregateRoot> _adapter;
        //private readonly IBusinessValidator _businessValidator;
        //private readonly ILoggerService _loggerService;

      public CrudService(IRepository<TAggregateRoot> repository, IAdapter<TDto, TAggregateRoot> adapter)
        {
            _repository = repository;
            _adapter = adapter;
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
            return aggregate != null ? _adapter.Adapt(aggregate) : default(TDto);
        }

        public Guid Create(TDto dto)
        {
            var aggregate = _adapter.Adapt(dto);
            //_businessValidator.Validate(aggregate)
            _repository.Add(aggregate);
            return aggregate.Id;
        }

        public void Update(Guid id, TDto dto)
        {
            var aggregate = _adapter.Adapt(dto);
            aggregate.Id = id;
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
