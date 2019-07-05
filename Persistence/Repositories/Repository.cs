using System.Collections.Generic;
using Domain.Contracts.Aggregates;

namespace Persistence.Repositories
{
  public class Repository : IRepository
  {
    public IAggregate GetById(int id)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<IAggregate> GetAll()
    {
      throw new System.NotImplementedException();
    }

    public void Update(IAggregate dto)
    {
      throw new System.NotImplementedException();
    }

    public void Remove(int id)
    {
      throw new System.NotImplementedException();
    }

    public void Add(IAggregate aggregate)
    {
      throw new System.NotImplementedException();
    }
  }
}
