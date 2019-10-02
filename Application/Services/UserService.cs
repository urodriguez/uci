using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Security.Authentication;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly TokenService _tokenService;

        public UserService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public string Login(UserLoginDto userLoginDto)
        {
            if (userLoginDto == null) return null;

            if (userLoginDto.UserName == "admin" && userLoginDto.Password == "admin") return _tokenService.GenerateJwtToken(userLoginDto.UserName);

            return null;
        }
    }
}