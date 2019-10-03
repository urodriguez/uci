using System;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        string Login(UserLoginDto userLoginDto);
        void ConfirmEmail(Guid id);
    }
}