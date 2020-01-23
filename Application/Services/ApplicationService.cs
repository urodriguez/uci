using System;
using System.Data;
using Domain.Contracts.Infrastructure.Crosscutting;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public abstract class ApplicationService
    {
        private readonly ITokenService _tokenService;

        protected ApplicationService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected IApplicationResult Execute<TResult>(Func<TResult> service, bool requireAuthorization = true) where TResult : IApplicationResult
        {
            try
            {
                if (requireAuthorization) CheckAuthorization();

                var serviceResult = service.Invoke();

                //return serviceResult is EmptyResult ? (IHttpActionResult)Ok() : Ok(serviceResult);
                return serviceResult;
            }
            catch (SecurityTokenValidationException stve)
            {
                //log

                return new ApplicationResult<string>
                {
                    Status = 2,
                    Message = "Authorization fails. Check credentials"
                };
            }
            catch (UnauthorizedAccessException uae)
            {
                //log
                throw;
            }
            catch (ObjectNotFoundException onfe)
            {
                //log
                throw;
            }
            catch (Exception e)
            {
                //log
                throw;
            }
        }

        protected void CheckAuthorization()
        {
            var validatedToken = _tokenService.Validate(InventAppContext.SecurityToken);
            InventAppContext.UserName = validatedToken.Subject.Name;
        }
    }
}