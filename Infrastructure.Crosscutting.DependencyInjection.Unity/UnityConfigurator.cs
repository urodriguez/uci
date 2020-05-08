using System.Web.Http;
using Application.BusinessValidators;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Factories;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Infrastructure.Persistence.Repositories;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Domain.Predicates.Factories;
using Domain.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.AutoMapping.AutoMapper;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Reporting;
using Infrastructure.Persistence.Dapper;
using Infrastructure.Persistence.Dapper.Repositories;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace Infrastructure.Crosscutting.DependencyInjection.Unity
{
    public class UnityConfigurator
    {
        private static IUnityContainer _container;

        public static void Configure(HttpConfiguration httpConfiguration)
        {
            _container = new UnityContainer();

            #region APPLICATION
            //Services
            _container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            _container.RegisterType<IProductTypeService, ProductTypeService>(new PerThreadLifetimeManager());
            _container.RegisterType<IReportService, ReportService>(new PerThreadLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new PerThreadLifetimeManager());

            //Adapters
            _container.RegisterType<IProductFactory, ProductFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<IProductTypeFactory, ProductTypeFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<IUserFactory, UserFactory>(new PerThreadLifetimeManager());
            #endregion

            #region DOMAIN
            //BusinessValidators
            _container.RegisterType<IProductBusinessValidator, ProductBusinessValidator>(new PerThreadLifetimeManager());
            _container.RegisterType<IProductTypeBusinessValidator, ProductTypeBusinessValidator>(new PerThreadLifetimeManager());
            _container.RegisterType<IUserBusinessValidator, UserBusinessValidator>(new PerThreadLifetimeManager());

            //PredicateFactories
            _container.RegisterType<IProductPredicateFactory, ProductPredicateFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<IUserPredicateFactory, UserPredicateFactory>(new PerThreadLifetimeManager());

            //Services
            _container.RegisterType<IRoleService, RoleService>(new PerThreadLifetimeManager());
            #endregion

            #region INFRASTRUCTURE
            //Crosscutting
            _container.RegisterType<IAppSettingsService, AppSettingsService>(new PerThreadLifetimeManager());
            _container.RegisterType<IAuditService, AuditService>(new PerThreadLifetimeManager());
            _container.RegisterType<IEmailService, EmailService>(new PerThreadLifetimeManager());
            _container.RegisterType<ILogService, LogService>(new PerResolveLifetimeManager());
            _container.RegisterInstance<IMapper>(MapperFactory.GetConfiguredMapper().CreateMapper());
            _container.RegisterType<IQueueService, QueueService>(new PerThreadLifetimeManager());
            _container.RegisterType<IReportInfrastructureService, ReportInfrastructureService>(new PerThreadLifetimeManager());
            _container.RegisterType<ITokenService, TokenService>(new PerThreadLifetimeManager());

            //Persistence
            _container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager());
            _container.RegisterType<IProductTypeRepository, ProductTypeRepository>(new PerThreadLifetimeManager());
            _container.RegisterType<IUserRepository, UserRepository>(new PerThreadLifetimeManager());
            #endregion

            httpConfiguration.DependencyResolver = new UnityDependencyResolver(_container);
        }

        public static IUnityContainer GetConfiguredContainer() => _container;
    }
}
