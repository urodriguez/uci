using System;
using System.Collections.Generic;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.DependencyInjection.Unity;
using Owin;

namespace Infrastructure.Crosscutting.BackgroundProcessing.Hangfire
{
    public class HangfireConfigurator
    {
        private static readonly IAppSettingsService AppSettingsService = new AppSettingsService();

        private static IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(AppSettingsService.HangfireInventAppConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }

        public static void Configure(IGlobalConfiguration httpConfiguration, IAppBuilder app)
        {
            var container = UnityConfigurator.GetConfiguredContainer();
            httpConfiguration.UseActivator(new ContainerJobActivator(container));

            app.UseHangfireAspNet(GetHangfireServers);
            
            if (AppSettingsService.Environment.IsDev())
            {
                // If we are in Dev, always allow Hangfire access.
                app.UseHangfireDashboard("/hangfire");
            }
            else
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new InventAppHangfireDashboardAuthorizationFilter() }
                });
            }
        }
    }
}
