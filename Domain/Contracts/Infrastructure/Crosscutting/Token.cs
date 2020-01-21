using System;
using System.Security.Principal;

namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public class Token : IToken
    {
        public string Issuer { get; set; }
        public IIdentity Subject { get; set; }
        public DateTime? NotBefore { get; set; }
        public DateTime? Expires { get; set; }
    }
}