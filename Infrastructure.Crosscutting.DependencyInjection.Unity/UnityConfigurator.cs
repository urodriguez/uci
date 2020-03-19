using System.Web.Http;
using Application.BusinessValidators;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Factories;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Infrastructure.Crosscutting.AppSettings;
using Domain.Contracts.Infrastructure.Crosscutting.Auditing;
using Domain.Contracts.Infrastructure.Crosscutting.Authentication;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;
using Domain.Contracts.Infrastructure.Crosscutting.Mailing;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Domain.Predicates.Factories;
using Domain.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Mapping;
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

            //PredicateFactories
            container.RegisterType<IProductPredicateFactory, ProductPredicateFactory>(new PerThreadLifetimeManager());
            container.RegisterType<IUserPredicateFactory, UserPredicateFactory>(new PerThreadLifetimeManager());

            //Services
            container.RegisterType<IRoleService, RoleService>(new PerThreadLifetimeManager());
            #endregion

            #region INFRASTRUCTURE
            //Crosscutting
            container.RegisterType<IAppSettingsService, AppSettingsService>(new PerThreadLifetimeManager());
            container.RegisterType<IAuditService, AuditService>(new PerThreadLifetimeManager());
            container.RegisterType<IEmailService, EmailService>(new PerThreadLifetimeManager());
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
