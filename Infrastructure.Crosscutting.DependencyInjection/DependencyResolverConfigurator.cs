using System.Web.Http;
using Application.Adapters;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Repositories;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mapping;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Unity;
using Unity.WebApi;
using Unity.Lifetime;

namespace Infrastructure.Crosscutting.DependencyInjection
{
    public class DependencyResolverConfigurator
    {
        public static void Configure(HttpConfiguration config)
        {
            var container = new UnityContainer();

            //Application.Services
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeService, ProductTypeService>(new PerThreadLifetimeManager());

            //Application.Adapters
            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeAdapter, ProductTypeAdapter>(new PerThreadLifetimeManager());

            //Infrastructure.Crosscutting
            container.RegisterType<ILogService, NLogService>(new PerThreadLifetimeManager());
            container.RegisterInstance<IMapper>(MapperFactory.GetConfiguredMApper().CreateMapper());

            //Infrastructure.Persistence
            container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeRepository, ProductTypeRepository>(new PerThreadLifetimeManager());

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
