using Hangfire;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.BackgroundProcessing.Hangfire;
using Infrastructure.Crosscutting.DependencyInjection.Unity;
using Infrastructure.Crosscutting.Documentation.Swagger;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Dequeue;
using Infrastructure.Crosscutting.Queueing.Enqueue;
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

            var appSettingsService = new AppSettingsService();
            var enqueueService = new EnqueueService(appSettingsService);
            var logService = new LogService(appSettingsService, enqueueService);
            var auditService = new AuditService(logService, appSettingsService, enqueueService);
            var emailService = new EmailService(logService, appSettingsService, enqueueService);
            var dequeueService = new DequeueService(appSettingsService, logService, logService, auditService, emailService);

            RecurringJob.AddOrUpdate("delete-old-logs", () => logService.DeleteOldLogs(), Cron.Daily);
            RecurringJob.AddOrUpdate("dequeue", () => dequeueService.Execute(), Cron.Minutely);
        }
    }
}
