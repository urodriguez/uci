using System.Data;
using Infrastructure.Crosscutting.AppSettings;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence.Dapper
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IAppSettingsService _appSettingsService;

        public DbConnectionFactory(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        public IDbConnection GetSqlConnection()
        {
            CustomDbProfiler.Current.ProfilerContext.Reset();
            return ProfiledDbConnectionFactory.New(new SqlServerDbConnectionFactory(_appSettingsService.ConnectionString), CustomDbProfiler.Current);
        }
    }
}