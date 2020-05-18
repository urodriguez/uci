using System.IO;
using Application.Contracts.Factories;
using Application.Dtos;
using Domain.Aggregates;
using Infrastructure.Crosscutting.AppSettings;

namespace Application.Factories
{
    public class TemplateFactory : ITemplateFactory
    {
        private readonly IAppSettingsService _appSettingsService;

        public TemplateFactory(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        public EmailTemplateRendered CreateForUserCreated(User user)
        {
            var bodyTemplateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_created.html";
            var bodyTemplate = File.ReadAllText(bodyTemplateFilePath);
            var bodyTemplateRendered = bodyTemplate.Replace("{{ConfirmEmailUrl}}", $"{_appSettingsService.WebApiUrl}/users/{user.Id}/confirmEmail")
                                                        .Replace("{{FirstName}}", user.FirstName)
                                                        .Replace("{{LastName}}", user.LastName)
                                                        .Replace("{{UserName}}", user.Name)
                                                        .Replace("{{Password}}", user.Password)
                                                        .Replace("{{Role}}", user.RoleId.ToString());

            return new EmailTemplateRendered
            {
                Subject = "InventApp - User Created",
                Body = bodyTemplateRendered
            };
        }

        public string CreateForUserEmailConfirmed(User user)
        {
            var templateFilePath = $"{_appSettingsService.TemplatesDirectory}\\user_emailConfirmed.html";
            var template = File.ReadAllText(templateFilePath);

            var templateRendered = template.Replace("{{UserName}}", user.Name)
                                                 .Replace("{{InventAppClientUrl}}", _appSettingsService.ClientUrl);

            return templateRendered;
        }

        public EmailTemplateRendered CreateForUserPasswordLost(User user)
        {
            var bodyTemplateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_passwordLost.html";
            var bodyTemplate = File.ReadAllText(bodyTemplateFilePath);
            var bodyTemplateRendered = bodyTemplate.Replace("{{UserName}}", user.Name)
                                                        .Replace("{{Password}}", user.Password)
                                                        .Replace("{{InventAppClientUrl}}", _appSettingsService.ClientUrl);

            return new EmailTemplateRendered
            {
                Subject = "InventApp - Reset Password",
                Body = bodyTemplateRendered
            };
        }
    }
}