using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Authentication
{
    public interface ITokenService
    {
        SecurityToken Generate(TokenGenerateRequest tokenGenerateRequest);
        TokenValidation Validate(TokenValidateRequest tokenValidateRequest);
    }
}