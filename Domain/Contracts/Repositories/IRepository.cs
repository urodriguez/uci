using System.Collections.Generic;
using Domain.Contracts.Aggregates;

namespace Domain.Contracts.Repositories
{
  public interface IRepository
  {
    IAggregate GetById(int id);
    IEnumerable<IAggregate> GetAll();
    void Update(IAggregate dto);
    void Remove(int id);
    void Add(IAggregate aggregate);
  }
}
