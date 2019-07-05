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
using Application.Contracts;
using Application.Contracts.Adapters;
using Domain.Aggregates;
using System.Collections.Generic;

namespace WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
	      		var container = new UnityContainer();

            var productsSeed = new List<Product>
            {
              new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
              new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
              new Product { Id = 3, Name = "Hammer1", Category = "Hardware", Price = 16.99M },
              new Product { Id = 4, Name = "Hammer2", Category = "Toys", Price = 16 },
              new Product { Id = 5, Name = "Hammer3", Category = "Groceries", Price = 20 },
              new Product { Id = 6, Name = "Hammer4", Category = "Hardware", Price = 21 }
            };

            // register all your components with the container here
            container.RegisterType<IProductRepository, ProductRepository>(new PerThreadLifetimeManager(), new InjectionConstructor(productsSeed));
            container.RegisterType<ILoggerService, NLogService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductService, ProductService>(new PerThreadLifetimeManager());
            container.RegisterType<IProductAdapter, ProductAdapter>(new PerThreadLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}