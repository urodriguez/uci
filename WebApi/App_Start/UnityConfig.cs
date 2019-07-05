using System.Web.Http;
using Domain.Contracts.Repositories;
using Unity;
using Unity.WebApi;
using Persistence.Repositories;
using Unity.Injection;
using Unity.Lifetime;
using Persistence;
using Crosscutting.Logging;
using Application;
using Application.Services;
using Application.Adapters;
using Application.Contracts.Adapters;

namespace WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
	      		var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager(), new InjectionConstructor(new UciDbContext()));
            container.RegisterType<IRepository, Repository>(new PerThreadLifetimeManager(), new InjectionConstructor(new UciDbContext()));
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerThreadLifetimeManager());
            container.RegisterType<ILoggerService, NLogService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}