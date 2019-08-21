using System.Collections.Generic;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IProductService : ICrudService<ProductDto>
    {
        IEnumerable<ProductDto> GetCheapest(decimal maxPrice);
    }
}