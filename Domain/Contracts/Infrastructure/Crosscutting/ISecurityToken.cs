using System;

namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface ISecurityToken
    {
        string Issuer { get; }
        //IIdentity Subject { get; }
        //DateTime? NotBefore { get; }
        DateTime? Expires { get; }
        string Token { get; }
    }
}