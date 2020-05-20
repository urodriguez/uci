using System.Web.Http;
using Application.BusinessValidators;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Factories;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Infrastructure.Persistence;
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
using Infrastructure.Crosscutting.Queueing.Dequeue;
using Infrastructure.Crosscutting.Queueing.Dequeue.DequeueResolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using Infrastructure.Crosscutting.Reporting;
using Infrastructure.Persistence.Dapper;
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

            //Factories
            _container.RegisterType<IProductFactory, ProductFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<IProductTypeFactory, ProductTypeFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<ITemplateFactory, TemplateFactory>(new PerThreadLifetimeManager());
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
            _container.RegisterType<IAuditDequeueResolver, AuditService>(new PerThreadLifetimeManager());
            _container.RegisterType<IAuditService, AuditService>(new PerThreadLifetimeManager());
            _container.RegisterType<IDequeueService, DequeueService>(new PerThreadLifetimeManager());
            _container.RegisterType<IEmailDequeueResolver, EmailService>(new PerThreadLifetimeManager());
            _container.RegisterType<IEmailService, EmailService>(new PerThreadLifetimeManager());
            _container.RegisterType<IEnqueueService, EnqueueService>(new PerThreadLifetimeManager());
            _container.RegisterType<ILogDequeueResolver, LogService>(new PerThreadLifetimeManager());
            _container.RegisterType<ILogService, LogService>(new PerResolveLifetimeManager());
            _container.RegisterInstance<IMapper>(MapperFactory.GetConfiguredMapper().CreateMapper());
            _container.RegisterType<IReportInfrastructureService, ReportInfrastructureService>(new PerThreadLifetimeManager());
            _container.RegisterType<ITokenService, TokenService>(new PerThreadLifetimeManager());

            //Persistence
            _container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new PerThreadLifetimeManager());
            _container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            #endregion

            httpConfiguration.DependencyResolver = new UnityDependencyResolver(_container);
        }

        public static IUnityContainer GetConfiguredContainer() => _container;

        public static T ResolveDependency<T>() => _container.Resolve<T>();
    }
}
