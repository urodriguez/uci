using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Aggregates;
using Infrastructure.Crosscutting.Renderting;

namespace Application.Contracts.Factories
{
    public interface ITemplateFactory
    {
        Task<Template> CreateForUserReportRequestedAsync(User user);
        Task<Template> CreateForUserEmailConfirmedAsync(User user);
        Task<Template> CreateForInventionReportAsync(IEnumerable<Invention> inventions);
        Task<Template> CreateForUserCreatedAsync(User user);
        Task<Template> CreateForUserPasswordLostAsync(User user);
    }
}