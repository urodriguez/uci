using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Application.ApplicationResults;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Exceptions;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Infrastructure.Crosscutting.Security.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly IUserPredicateFactory _userPredicateFactory;

        public UserService(
            IUserRepository userRepository, 
            IUserFactory factory, 
            IAuditService auditService, 
            TokenService tokenService, 
            IRoleService roleService, 
            IUserBusinessValidator userBusinessValidator, 
            IUserPredicateFactory userPredicateFactory,
            ILogService logService
        ) : base(
            roleService,
            userRepository, 
            factory, 
            auditService, 
            userBusinessValidator,
            tokenService,
            logService
        )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userPredicateFactory = userPredicateFactory;
        }

        public IApplicationResult Login(UserLoginDto userLoginDto)
        {
            return Execute(() =>
            {
                if (userLoginDto == null) throw new InvalidDataException();

                var byName = _userPredicateFactory.CreateByName(userLoginDto.UserName);
                var user = _userRepository.Get(byName).Single();

                if (user.IsLocked()) throw new UserLockedException(user.Name);

                //TODO use encrypted password
                if (!user.PasswordIsValid(userLoginDto.Password)) throw new AuthenticationFailException();

                if (!user.EmailConfirmed) throw new UserEmailNotConfirmedException(user.Name);

                user.LastLoginTime = DateTime.UtcNow;
                _userRepository.Update(user);

                var securityToken = _tokenService.Generate(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userLoginDto.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    }
                );

                if (securityToken == null) throw new InternalServerException("SecurityToken could not be generated");

                return new ApplicationResult<string>
                {
                    Status = ApplicationStatus.Ok,
                    Message = "Token generated",
                    Data = securityToken.Token
                };
            }, false);
        }

        public IApplicationResult ConfirmEmail(Guid id)
        {
            return Execute(() =>
            {
                var user = _userRepository.GetById(id);

                if (user == null) throw new ObjectNotFoundException($"User with Id={id} not found");

                user.EmailConfirmed = true;

                _userRepository.Update(user);

                return new EmptyResult
                {
                    Status = ApplicationStatus.Ok,
                    Message = "Email confirmed"
                };
            });
        }
    }
}