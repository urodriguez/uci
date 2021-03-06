﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Contracts.Factories;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Infrastructure.Redering;
using Domain.Aggregates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Application.Factories
{
    public class TemplateFactory : ITemplateFactory
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly JsonSerializer _jsonSerializer;

        public TemplateFactory(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
            _jsonSerializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        public async Task<Template> CreateForUserReportRequestedAsync(User user)
        {
            var templateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_reportRequested.html";
            var templateFileContent = await ReadFileAsync(templateFilePath);

            var dataBound = new JObject
            {
                ["userFullName"] = $"{user.FirstName} {user.LastName}"
            }.ToString();

            return new Template
            {
                Content = templateFileContent,
                DataBound = dataBound,
                Type = TemplateType.Html
            };
        }

        public async Task<Template> CreateForUserEmailConfirmedAsync(User user)
        {
            var templateFilePath = $"{_appSettingsService.TemplatesDirectory}\\user_emailConfirmed.html";
            var templateFileContent = await ReadFileAsync(templateFilePath);

            var dataBound = new JObject
            {
                ["userFullName"] = $"{user.FirstName} {user.LastName}",
                ["inventAppClientUrl"] = _appSettingsService.ClientUrl
            }.ToString();

            return new Template
            {
                Content = templateFileContent,
                DataBound = dataBound,
                Type = TemplateType.Html
            };
        }

        public async Task<Template> CreateForInventionReportAsync(IEnumerable<Invention> inventions)
        {
            var templateFilePath = $"{_appSettingsService.ReportsTemplatesDirectory}\\invention_report.html";
            var templateFileContent = await ReadFileAsync(templateFilePath);

            var dataBound = new JObject
            {
                ["inventions"] = (JArray)JToken.FromObject(inventions, _jsonSerializer)
            }.ToString();

            return new Template
            {
                Content = templateFileContent,
                DataBound = dataBound,
                Type = TemplateType.Pdf
            };
        }

        public async Task<Template> CreateForUserCreatedAsync(User user)
        {
            var templateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_created.html";
            var templateFileContent = await ReadFileAsync(templateFilePath);

            var dataBound = new JObject
            {
                ["confirmEmailUrl"] = $"{_appSettingsService.WebApiUrl}/v1.0/users/{user.Id}/confirmEmail",
                ["userFirstName"] = user.FirstName,
                ["userLastName"] = user.LastName,
                ["userPassword"] = user.Password,
                ["userRole"] = user.Role.ToString()
            }.ToString();

            return new Template
            {
                Content = templateFileContent,
                DataBound = dataBound,
                Type = TemplateType.Html
            };
        }

        public async Task<Template> CreateForUserForgotPasswordAsync(User user)
        {
            var templateFilePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_forgotPassword.html";
            var templateFileContent = await ReadFileAsync(templateFilePath);

            var dataBound = new JObject
            {
                ["userFullName"] = $"{user.GetFullName()}",
                ["userPassword"] = user.Password,
                ["inventAppClientUrl"] = _appSettingsService.ClientUrl
            }.ToString();

            return new Template
            {
                Content = templateFileContent,
                DataBound = dataBound,
                Type = TemplateType.Html
            };
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