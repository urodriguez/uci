using System;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        IApplicationResult Login(UserLoginDto userLoginDto);
        IApplicationResult ConfirmEmail(Guid id);
    }
}