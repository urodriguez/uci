using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnectionFactory dbConnectionFactory, ILogService loggerService) : base(dbConnectionFactory, loggerService)
        {
        }
    }
}