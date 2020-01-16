namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username);
    }
}