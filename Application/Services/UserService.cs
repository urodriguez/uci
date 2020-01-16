using System;
using System.Data;
using System.Linq;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.Security.Authentication;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly IRoleService _roleService;
        private readonly IUserPredicateFactory _userPredicateFactory;

        public UserService(
            IUserRepository userRepository, 
            IUserFactory factory, 
            IAuditService auditService, 
            TokenService tokenService, 
            IRoleService roleService, 
            IUserBusinessValidator userBusinessValidator, 
            IUserPredicateFactory userPredicateFactory
        ) : base(
            userRepository, 
            factory, 
            auditService, 
            userBusinessValidator
        )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _roleService = roleService;
            _userPredicateFactory = userPredicateFactory;
        }

        public new Guid Create(UserDto userDto)
        {
            if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();

            return base.Create(userDto);
        }

        public new void Update(Guid id, UserDto userDto)
        {
            if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();

            base.Update(id, userDto);
        }

        public new void Delete(Guid id)
        {
            if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();

            base.Delete(id);
        }

        public string Login(UserLoginDto userLoginDto)
        {
            if (userLoginDto == null) return null;

            var byName = _userPredicateFactory.CreateByName(userLoginDto.UserName);
            var user = _userRepository.Get(byName).Single();

            if (user.IsLocked()) return null;

            //TODO use encrypted password
            if (!user.PasswordIsValid(userLoginDto.Password)) return null;

            if (!user.EmailConfirmed) return null;

            user.LastLoginTime = DateTime.UtcNow;
            _userRepository.Update(user);
            
            return _tokenService.GenerateJwtToken(userLoginDto.UserName);
        }

        public void ConfirmEmail(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user == null) throw new ObjectNotFoundException();

            user.EmailConfirmed = true;

            _userRepository.Update(user);
        }
    }
}