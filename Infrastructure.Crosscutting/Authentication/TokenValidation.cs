using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenValidation
    {
        public TokenStatus TokenStatus { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        public bool TokenIsInvalid() => TokenStatus == TokenStatus.Invalid;
        public bool TokenIsExpired() => TokenStatus == TokenStatus.Expirated;
    }
}