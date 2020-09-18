using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Contracts.DuplicateValidators
{
    public interface IDuplicateValidator<TDto> where TDto : ICrudDto
    {
        Task ValidateAsync(TDto inventionCategoryDto);
    }
}