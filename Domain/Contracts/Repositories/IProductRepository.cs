using System.Collections.Generic;
using Domain.Aggregates;

namespace Domain.Contracts.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<Product> GetAll();
        //Product GetById(int id);
        //void Add(Product product);
        //void Update(Product product);
        //void Delete(int id);
        //void DeleteBulk(IEnumerable<int> idsList);
      IEnumerable<Product> GetCheapest(decimal maxPrice);
    }
}
