using System.Linq;
using System.Threading;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;

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

            var user = _userRepository.GetByField(x => x.Name, loggedUserName).Single();

            return user.IsAdmin();
        }
    }
}