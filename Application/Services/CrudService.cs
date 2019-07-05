using System.Collections.Generic;
using Application.Contracts;
using Application.Contracts.Adapters;
using Application.Dtos;
using Crosscutting.Logging;
using Domain.Contracts.Repositories;
using Persistence.Repositories;

namespace Application.Services
{
  public class CrudService : ICrudService
  {
    private readonly IRepository _repository;
    private readonly IAdapter _adapter;
    //private readonly ILoggerService _loggerService;
    //private readonly IBusinessValidator _businessValidator;

    public CrudService(IRepository repository, IAdapter adapter)
    {
      _repository = repository;
      _adapter = adapter;
      //_loggerService = loggerService;
    }

    public IDto GetById(int id)
    {
      var aggregate = _repository.GetById(id);
      var dto = _adapter.Adapt(aggregate);
      return dto;
    }

    public IEnumerable<IDto> GetAll()
    {
      var aggregates = _repository.GetAll();
      var dtos = _adapter.AdaptBulk(aggregates);
      return dtos;
    }

    public IDto Create(IDto dto)
    {
      var aggregate = _adapter.Adapt(dto);
      //_businessValidator.Validate(aggregate)
      _repository.Add(aggregate);
      //save changes
      return _adapter.Adapt(aggregate);
    }

    public IDto Update(IDto dto)
    {
      var aggregate = _adapter.Adapt(dto);
      //_businessValidator.Validate(aggregate)
      _repository.Update(aggregate);
      //save changes
      return _adapter.Adapt(aggregate);
    }

    public void Remove(int id)
    {
      _repository.Remove(id);
      //save changes
    }
  }
}
