namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenValidateRequest
    {
        public InfrastructureAccount Account { get; set; }
        public string SecurityToken { get; set; }
    }
}