using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.AggregateUpdaters;
using Application.Contracts.DuplicateValidators;
using Application.Contracts.Factories;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Auditing;
using Application.Contracts.Infrastructure.Authentication;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;

namespace Application.Services
{
    public class InventionService : CrudService<InventionDto, Invention>, IInventionService
    {
        private readonly IInventionPredicateFactory _inventionPredicateFactory;

        public InventionService(
            IRoleService roleService,
            IInventionFactory factory,
            IInventionUpdater updater,
            IAuditService auditService,
            IInventionDuplicateValidator inventionDuplicateValidator, 
            IInventionPredicateFactory inventionPredicateFactory,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            ILogService logService,
            IAppSettingsService appSettingsService,
            IInventAppContext inventAppContext
        ) : base(
            roleService,
            factory,
            updater,
            auditService, 
            inventionDuplicateValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService,
            inventAppContext
        )
        {
            _inventionPredicateFactory = inventionPredicateFactory;
        }

        public async Task<IApplicationResult> GetCheapestAsync(decimal maxPrice)
        {
            return await ExecuteAsync(async () =>
            {
                var byCheapest = _inventionPredicateFactory.CreateByCheapest(maxPrice);
                var cheapestInventions = await _unitOfWork.Inventions.GetAsync(byCheapest);

                var cheapestInventionsDto = _factory.CreateFromRange(cheapestInventions);

                return new OkApplicationResult<IEnumerable<InventionDto>>
                {
                    Data = cheapestInventionsDto
                };
            });
        }
    }
}
