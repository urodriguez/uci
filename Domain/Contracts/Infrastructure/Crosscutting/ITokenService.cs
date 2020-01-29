using System.Collections.Generic;
using System.Security.Claims;

namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface ITokenService
    {
        ISecurityToken Generate(IReadOnlyCollection<Claim> claims);
        IEnumerable<Claim> Validate(string securityToken);
    }
}