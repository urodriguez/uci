using System.Data;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ILogService logService, IAppSettingsService appSettingsService, IDbTransaction transaction) : base(logService, appSettingsService,transaction)
        {
        }
    }
}
