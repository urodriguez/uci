using Application.Adapters;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Services;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Persistence.Repositories;
using Unity;
using Unity.WebApi;
using Unity.Lifetime;

namespace Infrastructure.Crosscutting.DependencyInjection
{
    public class DependencyResolverFactory
    {
        public static UnityDependencyResolver GetConfiguredDependencyResolver()
        {
            var container = new UnityContainer();

            //Application.Services
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeService, ProductTypeService>(new PerThreadLifetimeManager());

            //Application.Adapters
            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeAdapter, ProductTypeAdapter>(new PerThreadLifetimeManager());

            //Infrastructure.Crosscutting
            container.RegisterType<ILoggerService, NLogService>(new PerThreadLifetimeManager());

            //Infrastructure.Persistence
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeRepository, ProductTypeRepository>(new PerThreadLifetimeManager());

            return new UnityDependencyResolver(container);
        }
    }
}
