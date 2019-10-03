using DapperExtensions;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Logging;

namespace Infrastructure.Persistence.Dapper.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnectionFactory dbConnectionFactory, ILogService loggerService) : base(dbConnectionFactory, loggerService)
        {
        }

        public User GetByName(string name)
        {
            return ExecuteGet(Predicates.Field<User>(u => u.Name, Operator.Eq, name));
        }
    }
}