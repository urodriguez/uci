using System.Threading.Tasks;
using Application.Infrastructure.Authentication;

namespace Application.Contracts.Infrastructure.Authentication
{
    public interface ITokenService
    {
        Task<TokenGenerateResponse> GenerateAsync(TokenGenerateRequest tokenGenerateRequest);
        Task<TokenValidateResponse> ValidateAsync(TokenValidateRequest tokenValidateRequest);
    }
}