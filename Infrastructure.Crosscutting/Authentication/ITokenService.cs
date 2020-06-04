using System.Threading.Tasks;

namespace Infrastructure.Crosscutting.Authentication
{
    public interface ITokenService
    {
        Task<TokenGenerateResponse> GenerateAsync(TokenGenerateRequest tokenGenerateRequest);
        Task<TokenValidateResponse> ValidateAsync(TokenValidateRequest tokenValidateRequest);
    }
}