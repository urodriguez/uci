using System;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<IApplicationResult> ConfirmEmailAsync(Guid id);
        Task<IApplicationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IApplicationResult> ForgotPasswordAsync(string email);
        Task<IApplicationResult> LoginAsync(UserCredentialDto userCredential);
        Task<IApplicationResult> GetLoggedAsync();
    }
}