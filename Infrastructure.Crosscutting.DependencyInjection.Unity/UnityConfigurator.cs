using System.Web.Http;
using Application.Adapters;
using Application.Contracts.Adapters;
using Application.Contracts.Services;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.Services;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mapping;
using Infrastructure.Crosscutting.Security.Authentication;
using Infrastructure.Persistence.Dapper;
using Infrastructure.Persistence.Dapper.Repositories;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace Infrastructure.Crosscutting.DependencyInjection.Unity
{
    public class UnityConfigurator
    {
        public static void Configure(HttpConfiguration config)
        {
            var container = new UnityContainer();

            //Application.Services
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeService, ProductTypeService>(new PerThreadLifetimeManager());
            container.RegisterType<IUserService, UserService>(new PerThreadLifetimeManager());

            //Application.Adapters
            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeAdapter, ProductTypeAdapter>(new PerThreadLifetimeManager());
            container.RegisterType<IUserAdapter, UserAdapter>(new PerThreadLifetimeManager());

            //Domain.Services
            container.RegisterType<IRoleService, RoleService>(new PerThreadLifetimeManager());

            //Infrastructure.Crosscutting
            container.RegisterType<ILogService, NLogService>(new PerThreadLifetimeManager());
            container.RegisterInstance<IMapper>(MapperFactory.GetConfiguredMapper().CreateMapper());
            container.RegisterType<ITokenService, TokenService>(new PerThreadLifetimeManager());

            //Infrastructure.Persistence
            container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeRepository, ProductTypeRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new PerThreadLifetimeManager());

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
