using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IProductService : ICrudService<ProductDto>
    {
        Task<IApplicationResult> GetCheapestAsync(decimal maxPrice);
    }
}