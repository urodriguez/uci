using System;
using System.Data;
using System.IO;
using System.Reflection;
using Application.ApplicationResults;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public abstract class ApplicationService
    {
        private readonly ITokenService _tokenService;
        protected readonly ILogService _logService;

        protected ApplicationService(ITokenService tokenService, ILogService logService)
        {
            _tokenService = tokenService;
            _logService = logService;
        }

        protected IApplicationResult Execute<TResult>(Func<TResult> service, bool requiresAuthentication = true) where TResult : IApplicationResult
        {
            try
            {
                if (requiresAuthentication) CheckAuthentication();

                var serviceResult = service.Invoke();

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Service Execution Succeed | message={serviceResult.Message}");

                return serviceResult;
            }
            catch (SecurityTokenValidationException stve)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | SecurityTokenValidationException | e.Message={stve.Message} - e.StackTrace={stve}");

                return new EmptyResult
                {
                    Status = ApplicationStatus.Unauthenticated,
                    Message = "Authentication fails. Check credentials"
                };
            }
            catch (UnauthorizedAccessException uae)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | UnauthorizedAccessException | e.Message={uae.Message} - e.StackTrace={uae}");

                return new EmptyResult
                {
                    Status = ApplicationStatus.Unauthorized,
                    Message = uae.Message
                };
            }
            catch (ObjectNotFoundException onfe)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | ObjectNotFoundException | e.Message={onfe.Message} - e.StackTrace={onfe}");

                return new EmptyResult
                {
                    Status = ApplicationStatus.NotFound,
                    Message = onfe.Message
                };
            }
            catch (InvalidDataException ide)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | InvalidDataException | e.Message={ide.Message} - e.StackTrace={ide}");

                return new EmptyResult
                {
                    Status = ApplicationStatus.BadRequest,
                    Message = "Missing data to process the request"
                };
            }
            catch (BusinessRuleException bre)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | BusinessRuleException | e.Message={bre.Message} - e.StackTrace={bre}");

                return new EmptyResult
                {
                    Status = ApplicationStatus.BadRequest,
                    Message = bre.Message
                };
            }
            catch (Exception e)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Exception | e.Message={e.Message} - e.StackTrace={e}");

                return new EmptyResult
                {
                    Status = ApplicationStatus.InternalServerError,
                    Message = e.Message
                };
            }
        }

        protected void CheckAuthentication()
        {
            var validatedToken = _tokenService.Validate(InventAppContext.SecurityToken);
            InventAppContext.UserName = validatedToken.Subject.Name;
        }
    }
}