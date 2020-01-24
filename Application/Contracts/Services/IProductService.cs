using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IProductService : ICrudService<ProductDto>
    {
        IApplicationResult GetCheapest(decimal maxPrice);
    }
}