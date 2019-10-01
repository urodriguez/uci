namespace Infrastructure.Crosscutting.Security.Authentication
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username);
    }
}