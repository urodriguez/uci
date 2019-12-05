using System;
using System.Data;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Security.Authentication;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly IRoleService _roleService;

        public UserService(IUserRepository userRepository, IUserAdapter adapter, IAuditService auditService, TokenService tokenService, IRoleService roleService) : base(userRepository, adapter, auditService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _roleService = roleService;
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

            var user = _userRepository.GetByName(userLoginDto.UserName);

            //TODO return custom for UI redirect
            if (user.IsLocked()) return null;

            //TODO use encrypted password
            if (!user.PasswordIsValid(userLoginDto.Password)) return null;

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