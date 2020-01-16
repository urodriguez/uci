using System.Linq;
using System.Threading;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;

namespace Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPredicateFactory _userPredicateFactory;

        public RoleService(IUserRepository userRepository, IUserPredicateFactory userPredicateFactory)
        {
            _userRepository = userRepository;
            _userPredicateFactory = userPredicateFactory;
        }

        public bool LoggedUserIsAdmin()
        {
            var userNameFromLoggedUser = Thread.CurrentPrincipal.Identity.Name;

            var byName = _userPredicateFactory.CreateByName(userNameFromLoggedUser);
            var user = _userRepository.Get(byName).Single();

            return user.IsAdmin();
        }
    }
}