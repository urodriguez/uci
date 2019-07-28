using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Adapters
{
  public interface IProductAdapter : IAdapter<ProductDto, Product>
  {
  }
}
