using System;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface ICrudService<TDto> where TDto : ICrudDto
    {
        Task<IApplicationResult> GetAllAsync();
        Task<IApplicationResult> GetByIdAsync(Guid id);
        Task<IApplicationResult> CreateAsync(TDto dto);
        Task<IApplicationResult> UpdateAsync(TDto dto);
        Task<IApplicationResult> DeleteAsync(Guid id);
    }
}