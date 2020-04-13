using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Application.ApplicationResults;
using Application.Exceptions;
using Domain.Exceptions;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;

namespace Application.Services
{
    public abstract class ApplicationService
    {
        protected readonly ITokenService _tokenService;
        protected readonly ILogService _logService;
        protected readonly IAppSettingsService _appSettingsService;

        protected ApplicationService(ITokenService tokenService, ILogService logService, IAppSettingsService appSettingsService)
        {
            _tokenService = tokenService;
            _logService = logService;
            _appSettingsService = appSettingsService;
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
            catch (AuthenticationFailException afe)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | AuthenticationFailException");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.Unauthenticated,
                    Message = $"Authentication fails. Issue: {afe.Message}"
                };
            }
            catch (UnauthorizedAccessException uae)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | UnauthorizedAccessException | e.Message={uae.Message} - e.StackTrace={uae}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.Unauthorized,
                    Message = uae.Message
                };
            }
            catch (ObjectNotFoundException onfe)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | ObjectNotFoundException | e.Message={onfe.Message} - e.StackTrace={onfe}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.NotFound,
                    Message = onfe.Message
                };
            }
            catch (CredentialNotProvidedException cnpe)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | CredentialNotProvidedException | e.Message={cnpe.Message} - e.StackTrace={cnpe}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = "Missing data to process the request"
                };
            }
            catch (BusinessRuleException bre)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | BusinessRuleException | e.Message={bre.Message} - e.StackTrace={bre}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = bre.Message
                };
            }
            catch (CorrelationException ce)
            {
                var correlationId = $"CORR_ID_ERROR_{Guid.NewGuid()}";
                var msgToLog = $"CorrelationId = {correlationId} | {ce}";
                //TODO: log locally (example: local file, iis) 'msgToLog'

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.InternalServerError,
                    Message = $"An Internal Server Error has ocurred. Please contact with your administrator. CorrelationId = {correlationId}"
                };
            }            
            catch (InternalServerException ise)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | InternalServerException | e={ise}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.InternalServerError,
                    Message = $"An Internal Server Error has ocurred. Please contact with your administrator. CorrelationId = {_logService.GetCorrelationId()}"
                };
            }
            catch (Exception e)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Exception | e={e}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.InternalServerError,
                    Message = $"An Internal Server Error has ocurred. Please contact with your administrator. CorrelationId = {_logService.GetCorrelationId()}"
                };
            }
        }

        protected void CheckAuthentication()
        {
            var tokenValidation = _tokenService.Validate(new TokenValidateRequest
            {
                SecurityToken = InventAppContext.SecurityToken
            });

            if (tokenValidation.TokenIsInvalid()) throw new AuthenticationFailException("SecurityToken is invalid");
            if (tokenValidation.TokenIsExpired()) throw new AuthenticationFailException("SecurityToken is expired");

            var claimName = tokenValidation.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (claimName == null) throw new InternalServerException("Missing claim type 'name'");

            InventAppContext.UserName = claimName.Value;
        }
    }
}