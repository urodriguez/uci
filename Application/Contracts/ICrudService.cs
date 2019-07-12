using System;
using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts
{
    public interface ICrudService<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<IDto> GetAll();
        IDto GetById(Guid id);
        Guid Create(IDto dto);
        void Update(IDto dto);
        void Delete(Guid id);
        void DeleteRange(IEnumerable<int> ids);
    }
}