using System;
using System.IO;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Rendering;

namespace Application.Services
{
    public class ReportService : ApplicationService, IReportService
    {
        private readonly IInventionPredicateFactory _inventionPredicateFactory;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly ITemplateFactory _templateFactory;
        private readonly ITemplateService _templateService;
        private readonly IEmailFactory _emailFactory;
        private readonly IEmailService _emailService;

        public ReportService(
            ITokenService tokenService, 
            IUnitOfWork unitOfWork,
            ILogService logService, 
            IInventionPredicateFactory inventionPredicateFactory,
            IAppSettingsService appSettingsService,
            IUserPredicateFactory userPredicateFactory,
            ITemplateFactory templateFactory,
            ITemplateService templateService,
            IEmailFactory emailFactory,
            IEmailService emailService,
            IInventAppContext inventAppContext
        ) : base(
            tokenService,
            unitOfWork,
            logService,
            inventAppContext
        )
        {
            _inventionPredicateFactory = inventionPredicateFactory;
            _appSettingsService = appSettingsService;
            _userPredicateFactory = userPredicateFactory;
            _templateFactory = templateFactory;
            _templateService = templateService;
            _emailFactory = emailFactory;
            _emailService = emailService;

            Directory.CreateDirectory($"{_appSettingsService.ReportsDirectory}");
        }

        public async Task<IApplicationResult> CreateForInventionsAsync(ReportInventionDto reportInventionDto)
        {
            return await ExecuteAsync(async () =>
            {
                var byPriceRange = _inventionPredicateFactory.CreateByPriceRange(reportInventionDto.MinPrice, reportInventionDto.MaxPrice);
                var inventions = await _unitOfWork.Inventions.GetAsync(byPriceRange);

                var reportTemplate = await _templateFactory.CreateForInventionReportAsync(inventions);
                var reportTemplateRendered = await _templateService.RenderAsync<byte[]>(reportTemplate);

                if (reportTemplateRendered == null)
                {
                    return new EmptyResult
                    {
                        Status = ApplicationResultStatus.InternalServerError,
                        Message = "Report couldn't be generated"
                    };
                }

                if (reportInventionDto.SendByEmail)
                {
                    var byName = _userPredicateFactory.CreateByName(_inventAppContext.UserName);
                    var user = await _unitOfWork.Users.GetFirstAsync(byName);

                    var email = await _emailFactory.CreateForUserReportRequestedAsync(user, reportTemplateRendered);

                    _emailService.SendAsync(email);
                }

                //TODO: return to UI
                File.WriteAllBytes($"{_appSettingsService.ReportsDirectory}\\inventions_{Guid.NewGuid()}.pdf", reportTemplateRendered);

                return new OkEmptyResult();
            });
        }
    }
}