using System;

namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenGenerateResponse
    {
        public string Issuer { get; set; }
        public DateTime? Expires { get; set; }
        public string SecurityToken { get; set; }
    }
}