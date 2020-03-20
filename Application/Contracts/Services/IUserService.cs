using System;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        IApplicationResult Login(CredentialsDto credentialsDto);
        IApplicationResult ConfirmEmail(Guid id);
        IApplicationResult CustomPassword(Guid id, PasswordDto passwordDto);
    }
}