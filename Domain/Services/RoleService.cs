using System.Data;
using System.Linq;
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

        public bool IsAdmin(string userName)
        {
            var byName = _userPredicateFactory.CreateByName(userName);
            var user = _unitOfWork.Users.Get(byName).FirstOrDefault();

            if (user == null) throw new ObjectNotFoundException($"UserName={userName} not exists in database");

            return user.IsAdmin();
        }
    }
}