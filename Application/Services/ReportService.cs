using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.ApplicationResults;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Reporting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Application.Services
{
    public class ReportService : ApplicationService, IReportService
    {
        private readonly IReportInfrastructureService _reportInfrastructureService;
        private readonly IProductPredicateFactory _productPredicateFactory;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly IEmailService _emailService;

        public ReportService(
            ITokenService tokenService, 
            IUnitOfWork unitOfWork,
            ILogService logService, 
            IReportInfrastructureService reportInfrastructureService,
            IProductPredicateFactory productPredicateFactory,
            IAppSettingsService appSettingsService,
            IUserPredicateFactory userPredicateFactory, 
            IEmailService emailService
        ) : base(
            tokenService,
            unitOfWork,
            logService
        )
        {
            _reportInfrastructureService = reportInfrastructureService;
            _productPredicateFactory = productPredicateFactory;
            _appSettingsService = appSettingsService;
            _userPredicateFactory = userPredicateFactory;
            _emailService = emailService;

            Directory.CreateDirectory($"{_appSettingsService.ReportsDirectory}");
        }

        public IApplicationResult CreateForProducts(ReportProductDto reportProductDto)
        {
            return Execute(() =>
            {
                var byPriceRange = _productPredicateFactory.CreateByPriceRange(reportProductDto.MinPrice, reportProductDto.MaxPrice);
                var products = _unitOfWork.Products.Get(byPriceRange);

                var templatePath = $"{_appSettingsService.ReportsTemplatesDirectory}\\product_report.html";
                var template = File.ReadAllText(templatePath);

                var serializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var reportJsonData = new JObject();
                var productsJArray = (JArray)JToken.FromObject(products, serializer);
                reportJsonData["products"] = productsJArray;

                var report = new Report
                {
                    Template = template,
                    Data = reportJsonData.ToString()
                };

                var reportBytes = _reportInfrastructureService.Create(report);

                if (reportBytes == null)
                {
                    return new EmptyResult
                    {
                        Status = ApplicationResultStatus.InternalServerError,
                        Message = "Report couldn't be generated"
                    };
                }

                if (reportProductDto.SendByEmail)
                {
                    var byName = _userPredicateFactory.CreateByName(InventAppContext.UserName);
                    var user = _unitOfWork.Users.Get(byName).Single();

                    var userRequestReportEmailTemplatePath = $"{_appSettingsService.EmailsTemplatesDirectory}\\user_reportRequested.html";
                    var userRequestReportEmailTemplate = File.ReadAllText(userRequestReportEmailTemplatePath);
                    var userRequestReportEmailBody = userRequestReportEmailTemplate.Replace("{{UserName}}", user.FirstName);

                    _emailService.Send(new Email
                    {
                        UseCustomSmtpServer = false,
                        To = user.Email,
                        Subject = "Products Report",
                        Body = userRequestReportEmailBody,
                        Attachments = new List<Attachment>
                        {
                            new Attachment
                            {
                                FileContent = reportBytes,
                                FileName = $"products_{Guid.NewGuid()}.pdf"
                            }
                        }
                    });
                }

                //TODO: return to UI
                File.WriteAllBytes($"{_appSettingsService.ReportsDirectory}\\products_{Guid.NewGuid()}.pdf", reportBytes);

                return new OkEmptyResult();
            });
        }
    }
}