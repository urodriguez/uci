using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Adapters;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Aggregates;

namespace Application.Adapters
{
  public class ProductAdapter : IProductAdapter
  {
    public IDto Adapt(IAggregate aggregate)
    {
      var product = (Product) aggregate;
      return new ProductDto
      {
        Id = product.Id,
        Category = product.Category,
        Name = product.Name,
        Price = product.Price
      };
    }

    public IEnumerable<IDto> AdaptBulk(IEnumerable<IAggregate> aggregates)
    {
      return aggregates.Select(Adapt);
    }

    public IAggregate Adapt(IDto dto)
    {
      var productDto = (ProductDto) dto;
      return new Product
      {
        Id = productDto.Id,
        Category = productDto.Category,
        Name = productDto.Name,
        Price = productDto.Price
      };
    }

    public IEnumerable<IAggregate> AdaptBulk(IEnumerable<IDto> dtos)
    {
      return dtos.Select(Adapt);
    }
  }
}