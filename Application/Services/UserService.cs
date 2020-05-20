using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using Application.ApplicationResults;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Exceptions;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Newtonsoft.Json;
using Claim = Infrastructure.Crosscutting.Authentication.Claim;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly ITemplateFactory _templateFactory;
        private readonly IEmailService _emailService;

        public UserService(
            IUnitOfWork unitOfWork, 
            IUserFactory userFactory, 
            IAuditService auditService, 
            ITokenService tokenService, 
            IRoleService roleService, 
            IUserBusinessValidator userBusinessValidator, 
            IUserPredicateFactory userPredicateFactory,
            ILogService logService,
            ITemplateFactory templateFactory,
            IEmailService emailService,
            IAppSettingsService appSettingsService) 
        : base(
            roleService,
            userFactory, 
            auditService, 
            userBusinessValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService
        )
        {
            _userPredicateFactory = userPredicateFactory;
            _templateFactory = templateFactory;
            _emailService = emailService;
        }

        public IApplicationResult Login(CredentialsDto credentialsDto)
        {
            return Execute(() =>
            {
                if (credentialsDto == null) throw new CredentialNotProvidedException();

                var byName = _userPredicateFactory.CreateByName(credentialsDto.UserName);
                var user = _unitOfWork.Users.Get(byName).FirstOrDefault();

                if (user == null) throw new ObjectNotFoundException($"User '{credentialsDto.UserName}' not found");
                
                if (!user.EmailConfirmed) return new OkApplicationResult<LoginDto>
                {
                    Data = new LoginDto { Status = LoginStatus.UnconfirmedEmail }
                };

                if (!user.Activate) return new OkApplicationResult<LoginDto>
                {
                    Data = new LoginDto { Status = LoginStatus.Inactive }
                };

                if (user.IsLocked())
                {
                    user.GenerateDefaultPassword();
                    user.ResetAccessFailedCount();
                    _unitOfWork.Users.Update(user);

                    var emailTemplateRendered = _templateFactory.CreateForUserPasswordLost(user);
                    _emailService.Send(new Email
                    {
                        To = user.Email,
                        Subject = emailTemplateRendered.Subject,
                        Body = emailTemplateRendered.Body
                    });

                    return new OkApplicationResult<LoginDto>
                    {
                        Data = new LoginDto { Status = LoginStatus.Locked }
                    };
                }

                //TODO use encrypted password
                if (!user.PasswordIsValid(credentialsDto.Password))
                {
                    user.AccessFailedCount++;
                    _unitOfWork.Users.Update(user);
                    throw new AuthenticationFailException("Invalid credentials");
                }

                if (!user.IsUsingCustomPassword) return new OkApplicationResult<LoginDto>
                {
                    Data = new LoginDto { Status = LoginStatus.NonCustomPassword }
                };

                user.LastLoginTime = DateTime.UtcNow;
                user.ResetAccessFailedCount();
                _unitOfWork.Users.Update(user);

                var tokenGenerateRequest = _tokenService.Generate(new TokenGenerateRequest
                {
                    Expires = _appSettingsService.DefaultTokenExpiresTime,
                    Claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, credentialsDto.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    }
                });

                if (tokenGenerateRequest == null) throw new InternalServerException("SecurityToken could not be generated");

                return new OkApplicationResult<LoginDto>
                {
                    Data = new LoginDto
                    {
                        Status = LoginStatus.Success,
                        SecurityToken = tokenGenerateRequest.SecurityToken
                    }
                };
            }, false);
        }

        public override IApplicationResult Create(UserDto userDto)
        {
            return Execute(() =>
            {
                if (!_roleService.IsAdmin(InventAppContext.UserName)) 
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{InventAppContext.UserName}'");

                _businessValidator.Validate(userDto);

                var user = _factory.Create(userDto);
                user.GenerateDefaultPassword();

                _unitOfWork.GetRepository<User>().Add(user);

                _auditService.Audit(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = InventAppContext.UserName,
                    EntityId = user.Id.ToString(),
                    EntityName = user.GetType().Name,
                    Entity = JsonConvert.SerializeObject(user),
                    Action = AuditAction.Create
                });

                var emailTemplateRendered = _templateFactory.CreateForUserCreated(user);

                _emailService.Send(new Email
                {
                    To = user.Email,
                    Subject = emailTemplateRendered.Subject,
                    Body = emailTemplateRendered.Body
                });

                return new OkApplicationResult<Guid>
                {
                    Data = user.Id
                };
            });
        }

        public IApplicationResult ConfirmEmail(Guid id)
        {
            return Execute(() =>
            {
                var user = _unitOfWork.Users.GetById(id);

                if (user == null) throw new ObjectNotFoundException($"User with Id={id} not found");

                user.EmailConfirmed = true;

                _unitOfWork.Users.Update(user);

                var templateRendered = _templateFactory.CreateForUserEmailConfirmed(user);

                return new OkApplicationResult<string>
                {
                    Data = templateRendered
                };
            }, false);
        }

        public IApplicationResult CustomPassword(Guid id, PasswordDto passwordDto)
        {
            return Execute(() =>
            {
                var user = _unitOfWork.Users.GetById(id);

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
                _unitOfWork.Users.Update(user);

                return new OkEmptyResult();
            }, false);
        }
        
        public IApplicationResult ForgotPassword(string userName)
        {
            return Execute(() =>
            {
                var byUserName = _userPredicateFactory.CreateByName(userName);
                var user = _unitOfWork.Users.Get(byUserName).FirstOrDefault();

                if (user == null) throw new ObjectNotFoundException($"User with UserName={userName} not found");

                user.GenerateDefaultPassword();
                user.ResetAccessFailedCount();
                _unitOfWork.Users.Update(user);

                var emailTemplateRendered = _templateFactory.CreateForUserPasswordLost(user);
                _emailService.Send(new Email
                {
                    To = user.Email,
                    Subject = emailTemplateRendered.Subject,
                    Body = emailTemplateRendered.Body
                });

                return new OkEmptyResult();
            }, false);
        }
    }
}