using Application.Dtos;

namespace Application.Contracts.BusinessValidators
{
    public interface IBusinessValidator<TDto> where TDto : IDto
    {
        void Validate(TDto dto);
    }
}