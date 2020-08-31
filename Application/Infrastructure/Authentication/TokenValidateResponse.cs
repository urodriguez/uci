using System.Collections.Generic;

namespace Application.Infrastructure.Authentication
{
    public class TokenValidateResponse
    {
        public TokenStatus TokenStatus { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        public bool TokenIsInvalid() => TokenStatus == TokenStatus.Invalid;
        public bool TokenIsExpired() => TokenStatus == TokenStatus.Expirated;
    }
}