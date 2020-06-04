using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Contracts.BusinessValidators
{
    public interface IBusinessValidator<TDto> where TDto : IDto
    {
        Task ValidateAsync(TDto dto, Guid id = default(Guid));
    }
}