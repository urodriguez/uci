using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.AggregateUpdaters;
using Application.Contracts.DuplicateValidators;
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
using Infrastructure.Crosscutting.Renderting;
using Newtonsoft.Json;
using Claim = Infrastructure.Crosscutting.Authentication.Claim;

namespace Application.Services
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserPredicateFactory _userPredicateFactory;
        private readonly ITemplateFactory _templateFactory;
        private readonly ITemplateService _templateService;
        private readonly IEmailFactory _emailFactory;
        private readonly IEmailService _emailService;

        public UserService(
            IUnitOfWork unitOfWork, 
            IUserFactory userFactory,
            IUserUpdater updater,
            IAuditService auditService, 
            ITokenService tokenService, 
            IRoleService roleService, 
            IUserDuplicateValidator userDuplicateValidator, 
            IUserPredicateFactory userPredicateFactory,
            ILogService logService,
            ITemplateFactory templateFactory,
            ITemplateService templateService,
            IEmailFactory emailFactory,
            IEmailService emailService,
            IAppSettingsService appSettingsService,
            IInventAppContext inventAppContext) 
        : base(
            roleService,
            userFactory, 
            updater,
            auditService, 
            userDuplicateValidator,
            tokenService,
            unitOfWork,
            logService,
            appSettingsService,
            inventAppContext
        )
        {
            _userPredicateFactory = userPredicateFactory;
            _templateFactory = templateFactory;
            _templateService = templateService;
            _emailFactory = emailFactory;
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

                    var email = await _emailFactory.CreateForUserPasswordLostAsync(user);
                    _emailService.SendAsync(email);

                    return new OkApplicationResult<LoginDto>
                    {
                        Data = new LoginDto { Status = LoginStatus.Locked }
                    };
                }

                //TODO use encrypted password
                if (!user.HasPassword(credentialsDto.Password))
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
                var isAdmin = await _roleService.IsAdminAsync(_inventAppContext.UserName);
                if (!isAdmin)
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserName}'");

                await _duplicateValidator.ValidateAsync(userDto);

                var user = _factory.Create(userDto);

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

                var email = await _emailFactory.CreateForUserCreatedAsync(user);
                _emailService.SendAsync(email);

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

                var template = await _templateFactory.CreateForUserEmailConfirmedAsync(user);
                var templateRendered = await _templateService.RenderAsync<string>(template);

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

                if (!user.HasPassword(passwordDto.Default)) throw new AuthenticationFailException("Invalid credentials");

                if (user.Password == passwordDto.Custom) return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = "Default and Custom password can not be equals"
                };

                user.SetPassword(passwordDto.Custom);
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

                var email = await _emailFactory.CreateForUserPasswordLostAsync(user);
                _emailService.SendAsync(email);

                return new OkEmptyResult();
            }, false);
        }
    }
}