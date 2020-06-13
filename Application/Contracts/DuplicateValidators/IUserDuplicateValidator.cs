using Application.Dtos;

namespace Application.Contracts.DuplicateValidators
{
    public interface IUserDuplicateValidator : IDuplicateValidator<UserDto>
    {
    }
}