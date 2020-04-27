﻿using System.Collections.Generic;

namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenGenerateRequest
    {
        public InfrastructureCredential Credential { get; set; }
        public int Expires { get; set; }
        public IReadOnlyCollection<Claim> Claims { get; set; }
    }
}