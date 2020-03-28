namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenValidateRequest
    {
        public InfrastructureCredential Account { get; set; }
        public string SecurityToken { get; set; }
    }
}