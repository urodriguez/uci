using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Security.Authentication
{
    public class TokenValidateDto
    {
        public IEnumerable<ClaimDto> Claims { get; set; }
    }
}