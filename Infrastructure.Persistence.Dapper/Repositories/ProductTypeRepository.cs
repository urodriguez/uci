using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;
using Domain.Contracts.Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class ProductTypeRepository: Repository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(IDbConnectionFactory dbConnectionFactory, ILogService loggerService) : base(dbConnectionFactory, loggerService)
        {
        }
    }
}