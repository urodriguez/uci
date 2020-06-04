﻿using System.Web.Http;
using Application;
using Application.BusinessValidators;
using Application.Contracts;
using Application.Contracts.BusinessValidators;
using Application.Contracts.Factories;
using Application.Contracts.Services;
using Application.Contracts.TemplateServices;
using Application.Factories;
using Application.Services;
using Application.TemplateServices;
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
            _container.RegisterType<IProductService, ProductService>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductTypeService, ProductTypeService>(new PerResolveLifetimeManager());
            _container.RegisterType<IReportService, ReportService>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new PerResolveLifetimeManager());

            //Core Services
            _container.RegisterType<ITemplateService, TemplateService>(new ContainerControlledLifetimeManager());

            //Factories
            _container.RegisterType<IProductFactory, ProductFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProductTypeFactory, ProductTypeFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserFactory, UserFactory>(new ContainerControlledLifetimeManager());

            //Contexts
            _container.RegisterType<IInventAppContext, InventAppContext>(new ContainerControlledLifetimeManager());
            #endregion

            #region DOMAIN
            //BusinessValidators
            _container.RegisterType<IProductBusinessValidator, ProductBusinessValidator>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductTypeBusinessValidator, ProductTypeBusinessValidator>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserBusinessValidator, UserBusinessValidator>(new PerResolveLifetimeManager());

            //PredicateFactories
            _container.RegisterType<IProductPredicateFactory, ProductPredicateFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserPredicateFactory, UserPredicateFactory>(new ContainerControlledLifetimeManager());

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
            _container.RegisterType<IReportInfrastructureService, ReportInfrastructureService>(new PerResolveLifetimeManager());
            _container.RegisterType<ITokenService, TokenService>(new PerResolveLifetimeManager());

            //Persistence
            _container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            #endregion

            httpConfiguration.DependencyResolver = new UnityDependencyResolver(_container);
        }

        public static IUnityContainer GetConfiguredContainer() => _container;

        public static T ResolveDependency<T>() => _container.Resolve<T>();
    }
}
