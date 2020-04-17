using Hangfire;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.BackgroundProcessing.Hangfire;
using Infrastructure.Crosscutting.DependencyInjection.Unity;
using Infrastructure.Crosscutting.Documentation.Swagger;
using Infrastructure.Crosscutting.Logging;
using Owin;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

namespace WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            UnityConfigurator.Configure(GlobalConfiguration.Configuration);
            SwaggerConfigurator.Configure(GlobalConfiguration.Configuration);
            HangfireConfigurator.Configure(Hangfire.GlobalConfiguration.Configuration, app);

            var logService = new LogService(new AppSettingsService());
            RecurringJob.AddOrUpdate("delete-old-logs", () => logService.DeleteOldLogs(), Cron.Daily);
        }
    }
}
