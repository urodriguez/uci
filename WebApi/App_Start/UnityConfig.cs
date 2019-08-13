using System.Web.Http;
using Domain.Contracts.Repositories;
using Unity;
using Unity.WebApi;
using Unity.Lifetime;
using Application.Services;
using Application.Adapters;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Persistence.Repositories;
using Crosscutting.Logging;

namespace WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
	      	var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeRepository, ProductTypeRepository>(new PerThreadLifetimeManager());

            container.RegisterType<ILoggerService, NLogService>(new PerThreadLifetimeManager());

            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeService, ProductTypeService>(new PerThreadLifetimeManager());

            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeAdapter, ProductTypeAdapter>(new PerThreadLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}