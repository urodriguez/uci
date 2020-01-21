using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Application;
using Domain.Contracts.Infrastructure.Crosscutting;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventAppApiController : ApiController
    {
        protected readonly ILogService _logService;

        public InventAppApiController(ILogService logService)
        {
            _logService = logService;
        }

        protected IHttpActionResult Execute<TResult>(Func<TResult> service)
        {
            try
            {
                InventAppContext.SecurityToken = ExtractToken(Request);

                var serviceResult = service.Invoke();

                return serviceResult is EmptyResult ? (IHttpActionResult) Ok() : Ok(serviceResult);
            }
            catch (SecurityTokenValidationException stve)
            {
                return Unauthorized();
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException onfe)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        private static string ExtractToken(HttpRequestMessage request)
        {
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1) return null;

            var bearerToken = authzHeaders.ElementAt(0);
            return bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
        }

        protected IHttpActionResult SendInternalServerError(Exception e)
        {
            _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | exception={e}");
            return InternalServerError(e);
        }
    }
}