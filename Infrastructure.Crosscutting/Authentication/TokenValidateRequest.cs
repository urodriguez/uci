namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenValidateRequest
    {
        public InfrastructureCredential Credential { get; set; }
        public string SecurityToken { get; set; }
    }
}