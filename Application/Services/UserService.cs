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
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Auditing;
using Application.Contracts.Infrastructure.Authentication;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Infrastructure.Mailing;
using Application.Contracts.Infrastructure.Redering;
using Application.Contracts.Services;
using Application.Dtos;
using Application.Exceptions;
using Application.Infrastructure.Auditing;
using Application.Infrastructure.Authentication;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Newtonsoft.Json;
using Claim = Application.Infrastructure.Authentication.Claim;

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
            IUserPredicateFactory userPredicateFactory,
            IUnitOfWork unitOfWork, 
            IUserFactory userFactory,
            IUserUpdater updater,
            IAuditService auditService, 
            ITokenService tokenService, 
            IRoleService roleService, 
            IUserDuplicateValidator userDuplicateValidator, 
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

        public async Task<IApplicationResult> LoginAsync(UserCredentialDto userCredential)
        {
            return await ExecuteAsync(async () =>
            {
                if (userCredential == null) return new ApplicationResult<LoginResultDto>
                {
                    Status = ApplicationResultStatus.Unauthenticated,
                    Data = new LoginResultDto { Status = LoginStatus.InvalidEmailOrPassword }
                };

                var byEmail = _userPredicateFactory.CreateByEmail(userCredential.Email);
                var user = await _unitOfWork.Users.GetFirstAsync(byEmail);

                if (user == null) return new ApplicationResult<LoginResultDto>
                {
                    Status = ApplicationResultStatus.Unauthenticated,
                    Data = new LoginResultDto { Status = LoginStatus.InvalidEmailOrPassword }
                };

                if (!user.EmailConfirmed) return new OkApplicationResult<LoginResultDto>
                {
                    Data = new LoginResultDto { Status = LoginStatus.UnconfirmedEmail }
                };

                if (!user.Active) return new OkApplicationResult<LoginResultDto>
                {
                    Data = new LoginResultDto { Status = LoginStatus.Inactive }
                };

                if (user.IsLocked())
                {
                    user.GenerateDefaultPassword();
                    user.ResetAccessFailedCount();
                    await _unitOfWork.Users.UpdateAsync(user);

                    var email = await _emailFactory.CreateForUserForgotPasswordAsync(user);
                    _emailService.SendAsync(email);

                    return new OkApplicationResult<LoginResultDto>
                    {
                        Data = new LoginResultDto { Status = LoginStatus.Locked }
                    };
                }

                //TODO use encrypted password
                if (!user.HasPassword(userCredential.Password))
                {
                    user.AccessFailedCount++;
                    await _unitOfWork.Users.UpdateAsync(user);

                    return new OkApplicationResult<LoginResultDto>
                    {
                        Data = new LoginResultDto { Status = LoginStatus.InvalidEmailOrPassword }
                    };
                }

                user.LastLoginTime = DateTime.UtcNow;
                user.ResetAccessFailedCount();
                await _unitOfWork.Users.UpdateAsync(user);

                var tokenGenerateResponse = await _tokenService.GenerateAsync(new TokenGenerateRequest
                {
                    Expires = _appSettingsService.DefaultTokenExpiresTime,
                    Claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.GetSurname())
                    }
                });

                if (tokenGenerateResponse == null) throw new InternalServerException("SecurityToken could not be generated");

                return new OkApplicationResult<LoginResultDto>
                {
                    Data = new LoginResultDto
                    {
                        Status = user.IsUsingCustomPassword ? LoginStatus.Success : LoginStatus.NonCustomPassword,
                        SecurityToken = new SecurityTokenDto
                        {
                            Token = tokenGenerateResponse.SecurityToken,
                            Expires = tokenGenerateResponse.Expires
                        }
                    }
                };
            }, false);
        }

        public async Task<IApplicationResult> GetLoggedAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _unitOfWork.Users.GetByIdAsync(_inventAppContext.UserId);

                if (user == null) throw new ObjectNotFoundException($"User with Id={_inventAppContext.UserId} not found");

                var userDto = await _factory.CreateAsync(user);

                return new OkApplicationResult<UserDto>
                {
                    Data = userDto
                };
            });
        }

        public override async Task<IApplicationResult> CreateAsync(UserDto userDto)
        {
            return await ExecuteAsync(async () =>
            {
                var isAdmin = await _roleService.IsAdminAsync(_inventAppContext.UserEmail);
                if (!isAdmin)
                    throw new UnauthorizedAccessException($"Access Denied. Check permissions for User '{_inventAppContext.UserEmail}'");

                await _duplicateValidator.ValidateAsync(userDto);

                var user = _factory.Create(userDto);

                await _unitOfWork.GetRepository<User>().AddAsync(user);

                _auditService.AuditAsync(new Audit
                {
                    Application = "InventApp",
                    Environment = _appSettingsService.Environment.Name,
                    User = _inventAppContext.UserEmail,
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

        public async Task<IApplicationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _unitOfWork.Users.GetByIdAsync(_inventAppContext.UserId);

                if (user == null) throw new ObjectNotFoundException($"User with Id={_inventAppContext.UserId} not found");

                if (!user.HasPassword(resetPasswordDto.OldPassword)) throw new AuthenticationFailException("Invalid OldPassword");

                if (user.Password == resetPasswordDto.NewPassword) return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = "Old and New password can not be equals"
                };

                user.SetPassword(resetPasswordDto.NewPassword);
                user.IsUsingCustomPassword = true;
                await _unitOfWork.Users.UpdateAsync(user);

                return new OkEmptyResult();
            });
        }
        
        public async Task<IApplicationResult> ForgotPasswordAsync(string email)
        {
            return await ExecuteAsync(async () =>
            {
                var byEmail = _userPredicateFactory.CreateByEmail(email);
                var user = await _unitOfWork.Users.GetFirstAsync(byEmail);

                if (user == null) throw new ObjectNotFoundException($"User with Email={email} not found");

                user.GenerateDefaultPassword();
                user.ResetAccessFailedCount();
                await _unitOfWork.Users.UpdateAsync(user);

                var sendEmailData = await _emailFactory.CreateForUserForgotPasswordAsync(user);
                _emailService.SendAsync(sendEmailData);

                return new OkEmptyResult();
            }, false);
        }
    }
}