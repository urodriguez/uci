using Application.Contracts;
using Application.Contracts.Adapters;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.Services
{
    public class ProductService : CrudService<Product>, IProductService
    {
      public ProductService(IProductRepository productRepository, IProductAdapter adapter) : base(productRepository, adapter)
      {
      }
    }
}
