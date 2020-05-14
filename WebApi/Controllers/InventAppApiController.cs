﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Application;
using Application.ApplicationResults;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using Microsoft.Owin;
using WebApi.HttpActionResults;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventAppApiController : ApiController
    {
        protected readonly ILogService _logService;
        private readonly IAppSettingsService _appSettingsService;

        public InventAppApiController(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
        }

        protected IHttpActionResult Execute<TResult>(Func<TResult> service, MediaType mediaType = MediaType.ApplicationJson) where TResult : IApplicationResult
        {
            InventAppContext.SecurityToken = ExtractToken(Request);

            var clientIpAddress = GetClientIpAddress(Request);
            if (_appSettingsService.Environment.IsTest() && !clientIpAddress.Equals("192.168.0.239"))//clientIpAddress == 192.168.0.239 => internal testing
                _appSettingsService.BaseInventAppApiUrl = "152.171.94.90";//for external requests that uses public ip 152.171.94.90

            var serviceResult = service.Invoke();

            switch (serviceResult.Status)
            {
                case ApplicationResultStatus.Ok:
                    if (serviceResult is EmptyResult) return Content(HttpStatusCode.OK, serviceResult.Message);
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
                    _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid ApplicationResultStatus(enum) value| serviceResult.Status={serviceResult.Status}");
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

        private static string GetClientIpAddress(HttpRequestMessage request)
        {
            const string httpContext = "MS_HttpContext";
            const string remoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
            const string owinContext = "MS_OwinContext";

            //Web-hosting
            if (request.Properties.ContainsKey(httpContext))
            {
                dynamic ctx = request.Properties[httpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            //Self-hosting
            if (request.Properties.ContainsKey(remoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[remoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            //Owin-hosting
            if (request.Properties.ContainsKey(owinContext))
            {
                dynamic ctx = request.Properties[owinContext];
                if (ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }

            return null;
        }
    }
}