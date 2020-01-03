using System.Web.Http;
using Application.BusinessValidators;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Factories;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.Services;
using Infrastructure.Crosscutting.Auditing;
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

            #region APPLICATION
            //Services
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeService, ProductTypeService>(new PerThreadLifetimeManager());
            container.RegisterType<IUserService, UserService>(new PerThreadLifetimeManager());

            //Adapters
            container.RegisterType<IProductFactory, ProductFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeFactory, ProductTypeFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IUserFactory, UserFactory>(new PerThreadLifetimeManager());
            #endregion

            #region DOMAIN
            //BusinessValidators
            container.RegisterType<IProductBusinessValidator, ProductBusinessValidator>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeBusinessValidator, ProductTypeBusinessValidator>(new PerThreadLifetimeManager());
            container.RegisterType<IUserBusinessValidator, UserBusinessValidator>(new PerThreadLifetimeManager());

            //Services
            container.RegisterType<IRoleService, RoleService>(new PerThreadLifetimeManager());
            #endregion

            #region INFRASTRUCTURE
            //Crosscutting
            container.RegisterType<IAuditService, AuditService>(new PerThreadLifetimeManager());
            container.RegisterType<ILogService, LogService>(new PerThreadLifetimeManager());
            container.RegisterInstance<IMapper>(MapperFactory.GetConfiguredMapper().CreateMapper());
            container.RegisterType<ITokenService, TokenService>(new PerThreadLifetimeManager());

            //Persistence
            container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IProductTypeRepository, ProductTypeRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new PerThreadLifetimeManager());
            #endregion

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
