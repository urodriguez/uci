using System.Collections.Generic;

namespace Application.Infrastructure.Authentication
{
    public class TokenGenerateRequest
    {
        public InfrastructureCredential Credential { get; set; }
        public int Expires { get; set; }
        public IReadOnlyCollection<Claim> Claims { get; set; }
    }
}