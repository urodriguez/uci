using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.Infrastructure.Logging;
using WebApi.HttpActionResults;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventAppApiController : ApiController
    {
        protected readonly ILogService _logService;
        private readonly IInventAppContext _inventAppContext;

        public InventAppApiController(ILogService logService, IInventAppContext inventAppContext)
        {
            _logService = logService;
            _inventAppContext = inventAppContext;
        }

        protected async Task<IHttpActionResult> ExecuteAsync<TResult>(Func<Task<TResult>> controllerPipeline, MediaType mediaType = MediaType.ApplicationJson) where TResult : IApplicationResult
        {
            _inventAppContext.SecurityToken = ExtractToken(Request);

            var serviceResult = await controllerPipeline.Invoke();

            switch (serviceResult.Status)
            {
                case ApplicationResultStatus.Ok:
                    if (serviceResult is EmptyResult) return Ok();
                    switch (mediaType)
                    {
                        case MediaType.ApplicationJson:
                            return Ok(((dynamic)serviceResult).Data);

                        case MediaType.TextHtml:
                            return new HtmlActionResult(((dynamic)serviceResult).Data, Request);

                        default:
                            return Ok(((dynamic)serviceResult).Data);
                    }

                case ApplicationResultStatus.BadRequest:
                    return Content(HttpStatusCode.BadRequest, serviceResult.Message);

                case ApplicationResultStatus.Unauthenticated:
                    return Content(HttpStatusCode.Unauthorized, serviceResult.Message);

                case ApplicationResultStatus.Unauthorized:
                    return Content(HttpStatusCode.Forbidden, serviceResult.Message);

                case ApplicationResultStatus.NotFound:
                    return Content(HttpStatusCode.NotFound, serviceResult.Message);

                case ApplicationResultStatus.UnsupportedMediaType:
                    return Content(HttpStatusCode.UnsupportedMediaType, serviceResult.Message);

                case ApplicationResultStatus.InternalServerError:
                    return Content(HttpStatusCode.InternalServerError, serviceResult.Message);

                default:
                    _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid ApplicationResultStatus(enum) value| serviceResult.Status={serviceResult.Status}");
                    return Content(HttpStatusCode.InternalServerError, $"An Internal Server Error has ocurred. Please contact with your administrator. CorrelationId = {_logService.GetCorrelationId()}");
            }
        }

        private static string ExtractToken(HttpRequestMessage request)
        {
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1) return null;

            var bearerToken = authzHeaders.ElementAt(0);
            return bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
        }
    }
}