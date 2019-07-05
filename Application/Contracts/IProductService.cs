using Domain.Aggregates;

namespace Application.Contracts
{
  public interface IProductService : ICrudService<Product>
  {
  }
}