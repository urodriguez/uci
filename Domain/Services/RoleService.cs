using System.Data;
using System.Threading.Tasks;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;

namespace Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPredicateFactory _userPredicateFactory;

        public RoleService(IUnitOfWork unitOfWork, IUserPredicateFactory userPredicateFactory)
        {
            _unitOfWork = unitOfWork;
            _userPredicateFactory = userPredicateFactory;
        }

        public async Task<bool> IsAdmin(string userName)
        {
            var byName = _userPredicateFactory.CreateByName(userName);
            var user = await _unitOfWork.Users.GetFirstAsync(byName);

            if (user == null) throw new ObjectNotFoundException($"UserName={userName} not exists in database");

            return user.IsAdmin();
        }
    }
}