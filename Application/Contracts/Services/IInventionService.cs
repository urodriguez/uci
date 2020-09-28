using System;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IInventionService : ICrudService<InventionDto>
    {
        Task<IApplicationResult> GetCheapestAsync(decimal maxPrice);
        Task<IApplicationResult> UpdateStateAsync(Guid id, InventionStateDto dto);
    }
}