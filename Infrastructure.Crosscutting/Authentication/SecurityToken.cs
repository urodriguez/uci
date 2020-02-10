using System;
using Domain.Contracts.Infrastructure.Crosscutting.Authentication;

namespace Infrastructure.Crosscutting.Authentication
{
    public class SecurityToken : ISecurityToken
    {
        public string Issuer { get; set; }
        public DateTime? Expires { get; set; }
        public string Token { get; set; }
    }
}