using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface IUserService
    {
        string Login(UserLoginDto userLoginDto);
    }
}