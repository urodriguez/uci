using System.IO;
using System.Threading.Tasks;
using Application.Contracts.TemplateServices;
using Application.Dtos;
using Domain.Aggregates;
using Infrastructure.Crosscutting.AppSettings;

namespace Application.TemplateServices
{
    public class TemplateService : ITemplateService
    {
        private readonly IAppSettingsService _appSettingsService;

        public TemplateService(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        public async Task<EmailTemplateRendered> RenderForUserCreatedAsync(User user)
        {
            var bodyTemplateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_created.html";
            var bodyTemplate = await ReadFileAsync(bodyTemplateFilePath);
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

        public async Task<string> RenderForUserEmailConfirmedAsync(User user)
        {
            var templateFilePath = $"{_appSettingsService.TemplatesDirectory}\\user_emailConfirmed.html";
            var template = await ReadFileAsync(templateFilePath);

            var templateRendered = template.Replace("{{UserName}}", user.Name)
                                                 .Replace("{{InventAppClientUrl}}", _appSettingsService.ClientUrl);

            return templateRendered;
        }

        public async Task<EmailTemplateRendered> RenderForUserPasswordLostAsync(User user)
        {
            var bodyTemplateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_passwordLost.html";
            var bodyTemplate = await ReadFileAsync(bodyTemplateFilePath);
            var bodyTemplateRendered = bodyTemplate.Replace("{{UserName}}", user.Name)
                                                        .Replace("{{Password}}", user.Password)
                                                        .Replace("{{InventAppClientUrl}}", _appSettingsService.ClientUrl);

            return new EmailTemplateRendered
            {
                Subject = "InventApp - Reset Password",
                Body = bodyTemplateRendered
            };
        }

        public async Task<EmailTemplateRendered> RenderForUserReportRequestedAsync(User user)
        {
            var bodyTemplateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_reportRequested.html";
            var bodyTemplate = await ReadFileAsync(bodyTemplateFilePath);
            var bodyTemplateRendered = bodyTemplate.Replace("{{UserName}}", user.FirstName);

            return new EmailTemplateRendered
            {
                Subject = "InventApp - Products Report",
                Body = bodyTemplateRendered
            };
        }

        public async Task<string> ReadForProductReportAsync()
        {
            var templatePath = $"{_appSettingsService.ReportsTemplatesDirectory}\\product_report.html";
            var template = await ReadFileAsync(templatePath);

            return template;
        }

        private async Task<string> ReadFileAsync(string filePath)
        {
            byte[] fileContentBytes;
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fsLenght = (int)fs.Length;
                fileContentBytes = new byte[fsLenght];
                await fs.ReadAsync(fileContentBytes, 0, fsLenght);
            }

            return System.Text.Encoding.ASCII.GetString(fileContentBytes);
        }
    }
}