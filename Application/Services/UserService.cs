using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Contracts.TemplateServices;
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
        private readonly ITemplateService _templateService;
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
            ITemplateService templateService,
            IEmailService emailService,
            IAppSettingsService appSettingsService,
            IInventAppContext inventAppContext
        ) 
        : base(
            roleService,
            userFactory, 
            auditService, 
            userBusinessValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService,
            inventAppContext
        )
        {
            _userPredicateFactory = userPredicateFactory;
            _templateService = templateService;
            _emailService = emailService;
        }

        public async Task<IApplicationResult> LoginAsync(CredentialsDto credentialsDto)
        {
            return await ExecuteAsync(async () =>
            {
                if (credentialsDto == null) throw new CredentialNotProvidedException();

                var byName = _userPredicateFactory.CreateByName(credentialsDto.UserName);
                var user = await _unitOfWork.Users.GetFirstAsync(byName);

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
                    await _unitOfWork.Users.UpdateAsync(user);

                    var emailTemplateRendered = await _templateService.RenderForUserPasswordLostAsync(user);
                    _emailService.SendAsync(new Email
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
                    await _unitOfWork.Users.UpdateAsync(user);

                    return new OkApplicationResult<LoginDto>
                    {
                        Data = new LoginDto { Status = LoginStatus.InvalidPassword }
                    };
                }

                if (!user.IsUsingCustomPassword) return new OkApplicationResult<LoginDto>
                {
                    Data = new LoginDto { Status = LoginStatus.NonCustomPassword }
                };

                user.LastLoginTime = DateTime.UtcNow;
                user.ResetAccessFailedCount();
                await _unitOfWork.Users.UpdateAsync(user);

                var tokenGenerateRequest = await _tokenService.GenerateAsync(new TokenGenerateRequest
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

        public override async Task<IApplicationResult> CreateAsync(UserDto userDto)
        {
            return await ExecuteAsync(async () =>
            {
                var isAdmin = await _roleService.IsAdmin(_inventAppContext.UserName);
                if (!isAdmin)
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserName}'");

                await _businessValidator.ValidateAsync(userDto);

                var user = _factory.Create(userDto);
                user.GenerateDefaultPassword();

                await _unitOfWork.GetRepository<User>().AddAsync(user);

                _auditService.AuditAsync(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = _inventAppContext.UserName,
                    EntityId = user.Id.ToString(),
                    EntityName = user.GetType().Name,
                    Entity = JsonConvert.SerializeObject(user),
                    Action = AuditAction.Create
                });

                var emailTemplateRendered = await _templateService.RenderForUserCreatedAsync(user);

                _emailService.SendAsync(new Email
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

        public async Task<IApplicationResult> ConfirmEmailAsync(Guid id)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

                if (user == null) throw new ObjectNotFoundException($"User with Id={id} not found");

                user.EmailConfirmed = true;

                await _unitOfWork.Users.UpdateAsync(user);

                var templateRendered = await _templateService.RenderForUserEmailConfirmedAsync(user);

                return new OkApplicationResult<string>
                {
                    Data = templateRendered
                };
            }, false);
        }

        public async Task<IApplicationResult> CustomPasswordAsync(Guid id, PasswordDto passwordDto)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);

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
                await _unitOfWork.Users.UpdateAsync(user);

                return new OkEmptyResult();
            }, false);
        }
        
        public async Task<IApplicationResult> ForgotPasswordAsync(string userName)
        {
            return await ExecuteAsync(async () =>
            {
                var byUserName = _userPredicateFactory.CreateByName(userName);
                var user = await _unitOfWork.Users.GetFirstAsync(byUserName);

                if (user == null) throw new ObjectNotFoundException($"User with UserName={userName} not found");

                user.GenerateDefaultPassword();
                user.ResetAccessFailedCount();
                await _unitOfWork.Users.UpdateAsync(user);

                var emailTemplateRendered = await _templateService.RenderForUserPasswordLostAsync(user);
                _emailService.SendAsync(new Email
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