using System.Data;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Logging;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class InventionTypeRepository: Repository<InventionType>, IInventionTypeRepository
    {
        public InventionTypeRepository(ILogService logService, IAppSettingsService appSettingsService, IDbTransaction transaction) : base(logService, appSettingsService, transaction)
        {
        }
    }
}