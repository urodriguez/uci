using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Authentication
{
    public interface ITokenService
    {
        TokenGenarateResponse Generate(TokenGenerateRequest tokenGenerateRequest);
        TokenValidateResponse Validate(TokenValidateRequest tokenValidateRequest);
    }
}