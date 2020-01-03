using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
  public interface IProductFactory : IFactory<ProductDto, Product>
  {
  }
}
