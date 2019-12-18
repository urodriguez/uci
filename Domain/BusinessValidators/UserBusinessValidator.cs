using Domain.Aggregates;
using Domain.Contracts.BusinessValidators;
using Domain.Contracts.Repositories;

namespace Domain.BusinessValidators
{
    public class UserBusinessValidator : IUserBusinessValidator
    {
        private readonly IUserRepository _userRepository;

        public UserBusinessValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Validate(User user)
        {
        }
    }
}