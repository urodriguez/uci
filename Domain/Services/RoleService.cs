using System.Data;
using System.Linq;
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

        public bool IsAdmin(string userName)
        {
            var byName = _userPredicateFactory.CreateByName(userName);
            var user = _userRepository.Get(byName).FirstOrDefault();

            if (user == null) throw new ObjectNotFoundException($"UserName={userName} not exists in database");

            return user.IsAdmin();
        }
    }
}