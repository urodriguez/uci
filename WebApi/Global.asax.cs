using System.Web.Http;
using Infrastructure.Crosscutting.DependencyInjection;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = DependencyResolverFactory.GetConfiguredDependencyResolver();
        }
    }
}
