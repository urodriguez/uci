namespace Infrastructure.Crosscutting.Authentication
{
    public interface ITokenService
    {
        TokenGenerateResponse Generate(TokenGenerateRequest tokenGenerateRequest);
        TokenValidateResponse Validate(TokenValidateRequest tokenValidateRequest);
    }
}