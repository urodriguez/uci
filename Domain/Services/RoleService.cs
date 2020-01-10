using System.Linq;
using System.Threading;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.Enums;
using Domain.Predicates;

namespace Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUserRepository _userRepository;

        public RoleService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool LoggedUserIsAdmin()
        {
            var loggedUserName = Thread.CurrentPrincipal.Identity.Name;

            var byName = new InventAppPredicateIndividual<User>(u => u.Name, InventAppPredicateOperator.Eq, loggedUserName);
            var user = _userRepository.Get(byName).Single();

            return user.IsAdmin();
        }
    }
}