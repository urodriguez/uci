using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
    public interface ITemplateFactory
    {
        EmailTemplateRendered CreateForUserCreated(User user);
        string CreateForUserEmailConfirmed(User user);
        EmailTemplateRendered CreateForUserPasswordLost(User user);
    }
}