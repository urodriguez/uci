using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Services
{
  public interface IProductService : ICrudService<ProductDto, Product>
  {
  }
}