using System.Data;
using System.Threading.Tasks;
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

        public async Task<IDbConnection> GetOpenedSqlConnectionAsync()
        {
            CustomDbProfiler.Current.ProfilerContext.Reset();

            var dbConnection = ProfiledDbConnectionFactory.New(new SqlServerDbConnectionFactory(_appSettingsService.ConnectionString), CustomDbProfiler.Current);
            await dbConnection.OpenAsync();

            return dbConnection;
        }
    }
}