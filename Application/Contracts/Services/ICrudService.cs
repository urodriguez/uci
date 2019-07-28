using System;
using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts.Services
{
    public interface ICrudService<TDto, TAggregateRoot> where TAggregateRoot : IAggregateRoot where TDto : IDto
    {
        IEnumerable<TDto> GetAll();
        TDto GetById(Guid id);
        Guid Create(TDto dto);
        void Update(Guid id, TDto dto);
        void Delete(Guid id);
        void DeleteRange(IEnumerable<Guid> ids);
    }
}