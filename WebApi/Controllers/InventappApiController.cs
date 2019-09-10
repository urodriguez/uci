using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventappApiController : ApiController
    {
        protected readonly ILogService _loggerService;

        public InventappApiController(ILogService loggerService)
        {
            _loggerService = loggerService;
        }

        protected IHttpActionResult Execute<TResult>(Func<TResult> service)
        {
            try
            {
                var serviceResult = service.Invoke();
                return SendOk(serviceResult);
            }
            catch (ObjectNotFoundException onfe)
            {
                return SendNotFound();
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        protected async Task<IHttpActionResult> ExecuteAsync<TResult>(Func<Task<TResult>> service)
        {
            try
            {
                var serviceResult = await service.Invoke();
                return SendOk(serviceResult);
            }
            catch (ObjectNotFoundException onfe)
            {
                return SendNotFound();
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        protected IHttpActionResult SendOk()
        {
            _loggerService.FlushQueueMessages();
            return Ok();
        }

        protected IHttpActionResult SendOk<T>(T data)
        {
            _loggerService.FlushQueueMessages();
            return Ok(data);
        }

        protected IHttpActionResult SendInternalServerError(Exception e)
        {
            _loggerService.QueueErrorMessage(e.ToString());
            _loggerService.FlushQueueMessages();
            return InternalServerError(e);
        }

        protected IHttpActionResult SendNotFound()
        {
            _loggerService.FlushQueueMessages();
            return NotFound();
        }
    }
}