using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Authentication
{
    public interface ITokenService
    {
        SecurityToken Generate(IReadOnlyCollection<Claim> claims);
        TokenValidation Validate(string securityToken);
    }
}