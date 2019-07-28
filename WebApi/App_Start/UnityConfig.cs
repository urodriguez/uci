using System.Web.Http;
using Domain.Contracts.Repositories;
using Unity;
using Unity.WebApi;
using Persistence.Repositories;
using Unity.Lifetime;
using Crosscutting.Logging;
using Application.Services;
using Application.Adapters;
using Application.Contracts;
using Application.Contracts.Adapters;
using Application.Contracts.Services;

namespace WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
	      	var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ILoggerService, NLogService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}