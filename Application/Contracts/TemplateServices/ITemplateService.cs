using System.Threading.Tasks;
using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.TemplateServices
{
    public interface ITemplateService
    {
        Task<EmailTemplateRendered> RenderForUserCreatedAsync(User user);
        Task<string> RenderForUserEmailConfirmedAsync(User user);
        Task<EmailTemplateRendered> RenderForUserPasswordLostAsync(User user);
        Task<EmailTemplateRendered> RenderForUserReportRequestedAsync(User user);
        Task<string> ReadForInventionReportAsync();
    }
}