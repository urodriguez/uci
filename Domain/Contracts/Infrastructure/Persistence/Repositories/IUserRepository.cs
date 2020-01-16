using Domain.Aggregates;

namespace Domain.Contracts.Infrastructure.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}