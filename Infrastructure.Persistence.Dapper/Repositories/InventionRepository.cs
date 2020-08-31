using System.Data;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Logging;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class InventionRepository : Repository<Invention>, IInventionRepository
    {
        public InventionRepository(ILogService logService, IAppSettingsService appSettingsService, IDbTransaction transaction) : base(logService, appSettingsService,transaction)
        {
        }
    }
}
