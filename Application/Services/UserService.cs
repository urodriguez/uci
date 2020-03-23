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
using Domain.Contracts.Infrastructure.Crosscutting.AppSettings;
using Domain.Contracts.Infrastructure.Crosscutting.Auditing;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;
using Domain.Contracts.Infrastructure.Crosscutting.Mailing;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Mailing;
using Claim = Infrastructure.Crosscutting.Authentication.Claim;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly IEmailService _emailService;

        public UserService(
            IUserRepository userRepository, 
            IUserFactory factory, 
            IAuditService auditService, 
            ITokenService tokenService, 
            IRoleService roleService, 
            IUserBusinessValidator userBusinessValidator, 
            IUserPredicateFactory userPredicateFactory,
            ILogService logService, 
            IEmailService emailService,
            IAppSettingsService appSettingsService
        ) : base(
            roleService,
            userRepository, 
            factory, 
            auditService, 
            userBusinessValidator,
            tokenService,
            logService,
            appSettingsService
        )
        {
            _userRepository = userRepository;
            _userPredicateFactory = userPredicateFactory;
            _emailService = emailService;
        }

        public IApplicationResult Login(CredentialsDto credentialsDto)
        {
            return Execute(() =>
            {
                if (credentialsDto == null) throw new InvalidDataException();

                var byName = _userPredicateFactory.CreateByName(credentialsDto.UserName);
                var user = _userRepository.Get(byName).Single();

                if (!user.EmailConfirmed) return new ApplicationResult<LoginDto>
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = $"User '{user.Name}' has not been confirmed his email yet",
                    Data = new LoginDto
                    {
                        Status = LoginStatus.UnconfirmedEmail
                    }
                };

                //TODO use encrypted password
                if (!user.PasswordIsValid(credentialsDto.Password)) throw new AuthenticationFailException("Invalid credentials");

                if (!user.IsUsingCustomPassword) return new ApplicationResult<LoginDto>
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = $"User '{user.Name}' has not set his custom password yet",
                    Data = new LoginDto
                    {
                        Status = LoginStatus.NonCustomPassword
                    }
                };

                if (user.IsLocked()) return new ApplicationResult<LoginDto>
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = $"User '{user.Name}' has his account locked",
                    Data = new LoginDto
                    {
                        Status = LoginStatus.Locked
                    }
                };

                user.LastLoginTime = DateTime.UtcNow;
                _userRepository.Update(user);

                var securityToken = _tokenService.Generate(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, credentialsDto.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    }
                );

                if (securityToken == null) throw new InternalServerException("SecurityToken could not be generated");

                return new ApplicationResult<LoginDto>
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = "Token generated",
                    Data = new LoginDto
                    {
                        Status = LoginStatus.Success,
                        Token = securityToken.Token
                    }
                };
            }, false);
        }

        public new IApplicationResult Create(UserDto userDto)
        {
            var applicationResult = base.Create(userDto);

            if (applicationResult.IsSuccessful())
            {
                var userId = ((ApplicationResult<Guid>)applicationResult).Data;

                var user = _userRepository.GetById(userId);
                user.GenerateDefaultPassword();
                _userRepository.Update(user);

                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var assetsPath = Path.GetFullPath(Path.Combine(appDirectory, @"..\Assets"));
                var templatePath = assetsPath + "\\templates\\user_created_email.html";
                var template = File.ReadAllText(templatePath);
                var templateReplaced = template.Replace("{{ConfirmEmailUrl}}", $"{_appSettingsService.WebApiUrl}/users/{userId}/confirmEmail")
                                               .Replace("{{FirstName}}", user.FirstName)
                                               .Replace("{{LastName}}", user.LastName)
                                               .Replace("{{UserName}}", user.Name)
                                               .Replace("{{Password}}", user.Password)
                                               .Replace("{{Role}}", user.RoleId.ToString());

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
                    To = user.Email,
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
                                               .Replace("{{InventAppClientUrl}}", _appSettingsService.ClientUrl);

                return new ApplicationResult<string>
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = "Email has been confirmed",
                    Data = templateReplaced
                };
            }, false);
        }

        public IApplicationResult CustomPassword(Guid id, PasswordDto passwordDto)
        {
            return Execute(() =>
            {
                var user = _userRepository.GetById(id);

                if (user == null) throw new ObjectNotFoundException($"User with Id={id} not found");

                if (!user.PasswordIsValid(passwordDto.Default)) throw new AuthenticationFailException("Invalid credentials");

                if (user.Password == passwordDto.Custom) return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = "Default and Custom password can not be equals"
                };

                if (!user.PasswordSatisfyComplexity(passwordDto.Custom)) return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = "Custom password does not satisfy the complexity"
                };

                user.Password = passwordDto.Custom;
                user.IsUsingCustomPassword = true;
                _userRepository.Update(user);

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.Ok,
                    Message = "Password has been updated"
                };
            }, false);
        }
    }
}