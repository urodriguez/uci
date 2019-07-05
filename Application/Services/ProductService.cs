using Application.Contracts.Adapters;
using Domain.Contracts.Repositories;
using Persistence.Repositories;

namespace Application.Services
{
    public class ProductService : CrudService, IProductService
    {
      public ProductService(IProductRepository repository, IProductAdapter adapter) : base(repository, adapter)
      {
      }
    }
}
