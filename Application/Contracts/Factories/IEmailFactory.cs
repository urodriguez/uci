using System.Threading.Tasks;
using Application.Contracts.Infrastructure.Mailing;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
    public interface IEmailFactory
    {
        Task<IEmail> CreateForUserReportRequestedAsync(User user, byte[] report);
        Task<IEmail> CreateForUserCreatedAsync(User user);
        Task<IEmail> CreateForUserForgotPasswordAsync(User user);
    }
}