using System.Web.Http;
using Application;
using Application.AggregateUpdaters;
using Application.Contracts;
using Application.Contracts.AggregateUpdaters;
using Application.Contracts.DuplicateValidators;
using Application.Contracts.Factories;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Auditing;
using Application.Contracts.Infrastructure.Authentication;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Infrastructure.Mailing;
using Application.Contracts.Infrastructure.Queueing.Dequeue;
using Application.Contracts.Infrastructure.Queueing.Enqueue;
using Application.Contracts.Infrastructure.Redering;
using Application.Contracts.Services;
using Application.DuplicateValidators;
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
using Infrastructure.Crosscutting.Queueing.Dequeue.Resolvers;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using Infrastructure.Crosscutting.Rendering;
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

            //https://github.com/unitycontainer/unity/wiki/Unity-Lifetime-Managers

            //TransientLifetimeManager: Unity creates and returns a new instance of the requested type for each call to the Resolve method
            //a new instance is created for each dependency on the constructors
            //TransientLifetimeManager is used by default for all types registered using the RegisterType method where no specific manager has been provided

            //PerResolveLifetimeManager: Similar to the TransientLifetimeManager, but it reuses the same object of registered type in the recursive object graph.
            //same instance is used for all dependencies on the contructors

            //ContainerControlledLifetimeManager: Unity creates a singleton object first time you call the Resolve or ResolveAll method
            //Then returns the same object on subsequent Resolve or ResolveAll calls.

            #region APPLICATION
            //Services
            _container.RegisterType<IInventionService, InventionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IInventionCategoryService, InventionCategoryService>(new PerResolveLifetimeManager());
            _container.RegisterType<IReportService, ReportService>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new PerResolveLifetimeManager());

            //Factories
            _container.RegisterType<IEmailFactory, EmailFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<IInventionFactory, InventionFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<IInventionCategoryFactory, InventionCategoryFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserFactory, UserFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<ITemplateFactory, TemplateFactory>(new PerResolveLifetimeManager());

            //DuplicateValidators
            _container.RegisterType<IInventionDuplicateValidator, InventionDuplicateValidator>(new PerResolveLifetimeManager());
            _container.RegisterType<IInventionCategoryDuplicateValidator, InventionCategoryDuplicateValidator>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserDuplicateValidator, UserDuplicateValidator>(new PerResolveLifetimeManager());

            //AggregateUpdaters
            _container.RegisterType<IInventionUpdater, InventionUpdater>(new PerResolveLifetimeManager());
            _container.RegisterType<IInventionCategoryUpdater, InventionCategoryUpdater>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserUpdater, UserUpdater>(new PerResolveLifetimeManager());

            //Contexts
            _container.RegisterType<IInventAppContext, InventAppContext>(new PerResolveLifetimeManager());
            #endregion

            #region DOMAIN
            //PredicateFactories
            _container.RegisterType<IInventionPredicateFactory, InventionPredicateFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<IInventionCategoryPredicateFactory, InventionCategoryPredicateFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserPredicateFactory, UserPredicateFactory>(new PerResolveLifetimeManager());

            //Services
            _container.RegisterType<IRoleService, RoleService>(new PerResolveLifetimeManager());
            #endregion

            #region INFRASTRUCTURE
            //Crosscutting
            _container.RegisterType<IAppSettingsService, AppSettingsService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAuditDequeueResolver, AuditService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAuditService, AuditService>(new PerResolveLifetimeManager());
            _container.RegisterType<IDequeueService, DequeueService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEmailDequeueResolver, EmailService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEmailService, EmailService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEnqueueService, EnqueueService>(new PerResolveLifetimeManager());
            _container.RegisterType<ILogDequeueResolver, LogService>(new PerResolveLifetimeManager());
            _container.RegisterType<ILogService, LogService>(new PerResolveLifetimeManager());
            _container.RegisterInstance<IMapper>(MapperFactory.GetConfiguredMapper().CreateMapper());//RegisterInstance always registers Singleton
            _container.RegisterType<ITemplateService, TemplateService>(new PerResolveLifetimeManager());
            _container.RegisterType<ITokenService, TokenService>(new PerResolveLifetimeManager());

            //Persistence
            _container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new PerResolveLifetimeManager());
            _container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            #endregion

            httpConfiguration.DependencyResolver = new UnityDependencyResolver(_container);
        }

        public static IUnityContainer GetConfiguredContainer() => _container;

        public static T ResolveDependency<T>() => _container.Resolve<T>();
    }
}
