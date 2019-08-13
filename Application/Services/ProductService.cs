using Application.Contracts;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.Services
{
    public class ProductService : CrudService<ProductDto, Product>, IProductService
    {
      public ProductService(IProductRepository repository, IProductAdapter adapter) : base(repository, adapter)
      {
      }
    }
}
