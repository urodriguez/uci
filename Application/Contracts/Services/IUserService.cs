using System;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<IApplicationResult> ConfirmEmailAsync(Guid id);
        Task<IApplicationResult> CustomPasswordAsync(Guid id, PasswordDto passwordDto);
        Task<IApplicationResult> ForgotPasswordAsync(string userName);
        Task<IApplicationResult> LoginAsync(UserCredentialDto userCredential);
    }
}