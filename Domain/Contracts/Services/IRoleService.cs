namespace Domain.Contracts.Services
{
    public interface IRoleService
    {
        bool IsAdmin(string userName);
    }
}