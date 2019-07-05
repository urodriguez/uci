using System.Collections.Generic;
using Application.Dtos;
using Domain.Contracts.Aggregates;

namespace Application.Contracts
{
    public interface ICrudService<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IEnumerable<IDto> GetAll();
        IDto GetById(int id);
        IDto Create(IDto dto);
        IDto Update(IDto dto);
        void Delete(int id);
        void DeleteRange(IEnumerable<int> ids);
    }
}