using System;

namespace Application.Infrastructure.Authentication
{
    public class TokenGenerateResponse
    {
        public string Issuer { get; set; }
        public DateTime? Expires { get; set; }
        public string SecurityToken { get; set; }
    }
}