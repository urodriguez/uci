using System;

namespace Domain.Contracts.Infrastructure.Crosscutting.Authentication
{
    public interface ISecurityToken
    {
        string Issuer { get; }
        DateTime? Expires { get; }
        string Token { get; }
    }
}