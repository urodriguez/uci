using System.Collections.Generic;
using Application.Dtos;

namespace Application.Contracts
{
  public interface ICrudService
  {
    IDto GetById(int id);
    IEnumerable<IDto> GetAll();
    IDto Create(IDto dto);
    IDto Update(IDto dto);
    void Remove(int id);
  }
}