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
using Domain.Contracts.Infrastructure.Crosscutting.Auditing;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;
using Domain.Contracts.Infrastructure.Crosscutting.Mailing;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Mailing;

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
                    new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(ClaimTypes.Name, userLoginDto.UserName),
                        new System.Security.Claims.Claim(ClaimTypes.Email, user.Email)
                    }
                );

                if (securityToken == null) throw new InternalServerException("SecurityToken could not be generated");

                return new ApplicationResult<string>
                {
                    Status = ApplicationResultStatus.Ok,
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
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var assetsPath = Path.GetFullPath(Path.Combine(appDirectory, @"..\Assets"));
                var templatePath = assetsPath + "\\templates\\user_created_email.html";
                var template = File.ReadAllText(templatePath);
                var userId = ((ApplicationResult<Guid>) applicationResult).Data;
                var templateReplaced = template.Replace("{{confirmEmailUrl}}", $"{InventAppContext.WebApiUrl()}/users/{userId}/confirmEmail")
                                               .Replace("{{FirstName}}", userDto.FirstName)
                                               .Replace("{{LastName}}", userDto.LastName)
                                               .Replace("{{UserName}}", userDto.Name)
                                               .Replace("{{Role}}", userDto.RoleId.ToString());

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
                    Body = templateReplaced
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

                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var assetsPath = Path.GetFullPath(Path.Combine(appDirectory, @"..\Assets"));
                var templatePath = assetsPath + "\\templates\\user_confirmed.html";
                var template = File.ReadAllText(templatePath);
                var templateReplaced = template.Replace("{{UserName}}", user.Name)
                                               .Replace("{{InventAppClientUrl}}", InventAppContext.ClientUrl());

                return new ApplicationResult<string>
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = "Email has been confirmed",
                    Data = templateReplaced
                };
            }, false);
        }
    }
}