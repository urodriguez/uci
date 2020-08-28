using System.Data;
using System.Threading.Tasks;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;

namespace Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUserPredicateFactory userPredicateFactory, IUnitOfWork unitOfWork)
        {
            _userPredicateFactory = userPredicateFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsAdminAsync(string email)
        {
            var byName = _userPredicateFactory.CreateByEmail(email);
            var user = await _unitOfWork.Users.GetFirstAsync(byName);

            if (user == null) throw new ObjectNotFoundException($"User Email={email} not exists in database");

            return user.IsAdmin();
        }
    }
}