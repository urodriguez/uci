using System.Threading.Tasks;
using Domain.Aggregates;
using Infrastructure.Crosscutting.Mailing;

namespace Application.Contracts.Factories
{
    public interface IEmailFactory
    {
        Task<Email> CreateForUserReportRequestedAsync(User user, byte[] report);
        Task<Email> CreateForUserCreatedAsync(User user);
        Task<Email> CreateForUserPasswordLostAsync(User user);
    }
}