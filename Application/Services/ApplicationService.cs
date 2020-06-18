using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Exceptions;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Exceptions;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;

namespace Application.Services
{
    public abstract class ApplicationService
    {
        protected readonly ITokenService _tokenService;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogService _logService;
        protected readonly IInventAppContext _inventAppContext;

        protected ApplicationService(ITokenService tokenService, IUnitOfWork unitOfWork, ILogService logService, IInventAppContext inventAppContext)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _logService = logService;
            _inventAppContext = inventAppContext;
        }

        protected async Task<IApplicationResult> ExecuteAsync<TResult>(Func<Task<TResult>> appServicePipeline, bool checkAuthentication = true) where TResult : IApplicationResult
        {
            try
            {
                if (checkAuthentication) 
                    await CheckAuthenticationAsync();

                await _unitOfWork.BeginTransactionAsync();

                var serviceResult = await appServicePipeline.Invoke();
                
                _unitOfWork.Commit();

                return serviceResult;
            }
            catch (AuthenticationFailException afe)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | AuthenticationFailException");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.Unauthenticated,
                    Message = $"Authentication fails. Issue: {afe.Message}"
                };
            }
            catch (UnauthorizedAccessException uae)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | UnauthorizedAccessException | e.Message={uae.Message} - e.StackTrace={uae}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.Unauthorized,
                    Message = uae.Message
                };
            }
            catch (ObjectNotFoundException onfe)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | ObjectNotFoundException | e.Message={onfe.Message} - e.StackTrace={onfe}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.NotFound,
                    Message = onfe.Message
                };
            }
            catch (ArgumentNullException ane)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | ArgumentNullException | e.Message={ane.Message} - e.StackTrace={ane}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = ane.Message
                };
            }
            catch (BusinessRuleException bre)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | BusinessRuleException | e.Message={bre.Message} - e.StackTrace={bre}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.BadRequest,
                    Message = bre.Message
                };
            }
            catch (InternalServerException ise)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | InternalServerException | e={ise}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.InternalServerError,
                    Message = $"An Internal Server Error has ocurred. Please contact with your administrator. CorrelationId = {_logService.GetCorrelationId()}"
                };
            }
            catch (Exception e)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Exception | e={e}");

                return new EmptyResult
                {
                    Status = ApplicationResultStatus.InternalServerError,
                    Message = $"An Internal Server Error has ocurred. Please contact with your administrator. CorrelationId = {_logService.GetCorrelationId()}"
                };
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        protected async Task CheckAuthenticationAsync()
        {
            var tokenValidation = await _tokenService.ValidateAsync(new TokenValidateRequest
            {
                SecurityToken = _inventAppContext.SecurityToken
            });

            if (tokenValidation.TokenIsInvalid()) throw new AuthenticationFailException("SecurityToken is invalid");
            if (tokenValidation.TokenIsExpired()) throw new AuthenticationFailException("SecurityToken is expired");

            var claimName = tokenValidation.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (claimName == null) throw new InternalServerException("Missing claim type 'name'");

            _inventAppContext.UserName = claimName.Value;
        }
    }
}