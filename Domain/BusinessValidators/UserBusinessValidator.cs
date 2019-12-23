using System;
using System.Linq;
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
            if (_userRepository.GetByField(u => u.Name, user.Name).Any()) throw new Exception($"User with name={user.Name} already exits");
            if (_userRepository.GetByField(u => u.Email, user.Email).Any()) throw new Exception($"User with email={user.Email} already exits");
        }
    }
}