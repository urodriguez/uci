using System.Web.Http;
using Infrastructure.Crosscutting.DependencyInjection.Unity;
using Infrastructure.Crosscutting.Documentation.Swagger;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfigurator.Configure(GlobalConfiguration.Configuration);
            SwaggerConfigurator.Configure(GlobalConfiguration.Configuration);
        }
    }
}
