using System.Threading.Tasks;

namespace Domain.Contracts.Services
{
    public interface IRoleService
    {
        Task<bool> IsAdmin(string userName);
    }
}