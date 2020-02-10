using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenValidate
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}