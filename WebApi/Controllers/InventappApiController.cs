using System;
using System.Data;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Domain.Contracts.Infrastructure.Crosscutting;

namespace WebApi.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventAppApiController : ApiController
    {
        protected readonly ILogService _loggerService;

        public InventAppApiController(ILogService loggerService)
        {
            _loggerService = loggerService;
        }

        protected IHttpActionResult Execute<TResult>(Func<TResult> service)
        {
            try
            {
                var serviceResult = service.Invoke();
                return serviceResult is EmptyResult ? (IHttpActionResult) Ok() : Ok(serviceResult);
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


        protected IHttpActionResult SendInternalServerError(Exception e)
        {
            _loggerService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | exception={e}");
            return InternalServerError(e);
        }
    }
}