using System;

namespace Infrastructure.Crosscutting.Authentication
{
    public class SecurityToken
    {
        public string Issuer { get; set; }
        public DateTime? Expires { get; set; }
        public string Token { get; set; }
    }
}