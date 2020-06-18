using System;

namespace Application.Dtos
{
    public class SecurityTokenDto
    {
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
    }
}