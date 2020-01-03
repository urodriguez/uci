using System;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Application.BusinessValidators
{
    public class UserBusinessValidator : BusinessValidator<UserDto, User>, IUserBusinessValidator
    {
        private readonly IUserRepository _userRepository;

        public UserBusinessValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override void ValidateFields(UserDto userDto)
        {
            if (_userRepository.GetByField(u => u.Name, userDto.Name).Any()) throw new Exception($"{AggregateRootName}: name={userDto.Name} already exits");

            if (!User.EmailIsValid(userDto.Email)) throw new Exception($"{AggregateRootName}: email={userDto.Email} has invalid email format");
            if (_userRepository.GetByField(u => u.Email, userDto.Email).Any()) throw new Exception($"{AggregateRootName}: email={userDto.Email} already exits");
        }
    }
}