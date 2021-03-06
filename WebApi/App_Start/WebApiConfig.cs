﻿using System.Web.Http;
using Newtonsoft.Json.Serialization;
using WebApi.Formatters;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.Add(new BrowserJsonFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "InventAppApiV1",
                routeTemplate: "api/v1.0/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
