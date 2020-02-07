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
using Domain.Contracts.Infrastructure.Crosscutting.Mailing;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Security.Authentication;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly IEmailService _emailService;

        public UserService(
            IUserRepository userRepository, 
            IUserFactory factory, 
            IAuditService auditService, 
            TokenService tokenService, 
            IRoleService roleService, 
            IUserBusinessValidator userBusinessValidator, 
            IUserPredicateFactory userPredicateFactory,
            ILogService logService, 
            IEmailService emailService
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
            _emailService = emailService;
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

        public new IApplicationResult Create(UserDto userDto)
        {
            var applicationResult = base.Create(userDto);

            if (applicationResult.IsSuccessful())
            {
                _emailService.Send(new Email
                {
                    UseCustomSmtpServer = true,
                    SmtpServerConfiguration = new SmtpServerConfiguration
                    {
                        Sender = new Sender
                        {
                            Name = "InventApp",
                            Email = "ucirod.infrastructure@gmail.com",
                            Password = "uc1r0d.1nfr4structur3"
                        },
                        Host = new Host
                        {
                            Name = "smtp.gmail.com",
                            Port = 465,
                            UseSsl = true
                        }
                    },
                    To = userDto.Email,
                    Subject = "User Created",
                    Body = $"Your user '{userDto.Name}' has been created. <br> Follow this link to confirm you email: www.confirm-email.com"
                });
            }

            return applicationResult;
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