using System;
using System.IO;
using Application.ApplicationResults;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Reporting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Application.Services
{
    public class ReportService : ApplicationService, IReportService
    {
        private readonly IReportInfrastructureService _reportInfrastructureService;
        private readonly IProductRepository _productRepository;
        private readonly IProductPredicateFactory _productPredicateFactory;
        private readonly IAppSettingsService _appSettingsService;

        public ReportService(
            ITokenService tokenService, 
            ILogService logService, 
            IReportInfrastructureService reportInfrastructureService, 
            IProductRepository productRepository,
            IProductPredicateFactory productPredicateFactory,
            IAppSettingsService appSettingsService
        ) : base(
            tokenService, 
            logService
        )
        {
            _reportInfrastructureService = reportInfrastructureService;
            _productRepository = productRepository;
            _productPredicateFactory = productPredicateFactory;
            _appSettingsService = appSettingsService;

            Directory.CreateDirectory($"{_appSettingsService.ReportsDirectory}");
        }

        public IApplicationResult CreateForProducts(ReportProductDto reportProductDto)
        {
            return Execute(() =>
            {
                var byPriceRange = _productPredicateFactory.CreateByPriceRange(reportProductDto.MinPrice, reportProductDto.MaxPrice);
                var products = _productRepository.Get(byPriceRange);

                var templatePath = $"{_appSettingsService.TemplatesDirectory}\\product_report.html";
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

                //TODO: return to UI
                File.WriteAllBytes($"{_appSettingsService.ReportsDirectory}\\products_{Guid.NewGuid()}.pdf", reportBytes);

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = "Report generated"
                };
            });
        }
    }
}