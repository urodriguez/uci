namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface ITokenService
    {
        string Generate(string username);
        IToken Validate(string securityToken);
    }
}