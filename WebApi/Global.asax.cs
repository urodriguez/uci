using System.Web.Http;
using Infrastructure.Crosscutting.DependencyInjection;
using Infrastructure.Crosscutting.Documentation.Swagger;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyResolverConfigurator.Configure(GlobalConfiguration.Configuration);
            SwaggerConfigurator.Configure(GlobalConfiguration.Configuration);
        }
    }
}
