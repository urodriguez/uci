using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.Services;
using Application.Contracts.TemplateServices;
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
        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;

        public ReportService(
            ITokenService tokenService, 
            IUnitOfWork unitOfWork,
            ILogService logService, 
            IReportInfrastructureService reportInfrastructureService,
            IProductPredicateFactory productPredicateFactory,
            IAppSettingsService appSettingsService,
            IUserPredicateFactory userPredicateFactory,
            ITemplateService templateService,
            IEmailService emailService,
            IInventAppContext inventAppContext
        ) : base(
            tokenService,
            unitOfWork,
            logService,
            inventAppContext
        )
        {
            _reportInfrastructureService = reportInfrastructureService;
            _productPredicateFactory = productPredicateFactory;
            _appSettingsService = appSettingsService;
            _userPredicateFactory = userPredicateFactory;
            _templateService = templateService;
            _emailService = emailService;

            Directory.CreateDirectory($"{_appSettingsService.ReportsDirectory}");
        }

        public async Task<IApplicationResult> CreateForProductsAsync(ReportProductDto reportProductDto)
        {
            return await ExecuteAsync(async () =>
            {
                var byPriceRange = _productPredicateFactory.CreateByPriceRange(reportProductDto.MinPrice, reportProductDto.MaxPrice);
                var products = await _unitOfWork.Products.GetAsync(byPriceRange);

                var productReportTemplate = await _templateService.ReadForProductReportAsync();

                var serializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var reportJsonData = new JObject();
                var productsJArray = (JArray)JToken.FromObject(products, serializer);
                reportJsonData["products"] = productsJArray;

                var report = new Report
                {
                    Template = productReportTemplate,
                    Data = reportJsonData.ToString()
                };

                var reportBytes = await _reportInfrastructureService.CreateAsync(report);

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
                    var byName = _userPredicateFactory.CreateByName(_inventAppContext.UserName);
                    var user = await _unitOfWork.Users.GetFirstAsync(byName);

                    var emailTemplateRendered = await _templateService.RenderForUserReportRequestedAsync(user);

                    _emailService.SendAsync(new Email
                    {
                        UseCustomSmtpServer = false,
                        To = user.Email,
                        Subject = emailTemplateRendered.Subject,
                        Body = emailTemplateRendered.Body,
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