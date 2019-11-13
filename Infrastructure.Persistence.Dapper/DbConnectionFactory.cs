using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence.Dapper
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetSqlConnection()
        {
            var envConnectionString = new Dictionary<string, string>
            {
                { "DEV", "Server=localhost;Database=UciRod.Inventapp;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True" },
                { "TEST", "Server=localhost;Database=UciRod.Inventapp_Test;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True" },
                { "STAGE", "Server=localhost;Database=UciRod.Inventapp_Stage;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True" },
                { "PROD", "Server=localhost;Database=UciRod.Inventapp_Prod;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True" }
            };

            var connectionString = envConnectionString[ConfigurationManager.AppSettings["Environment"]];

            CustomDbProfiler.Current.ProfilerContext.Reset();
            return ProfiledDbConnectionFactory.New(new SqlServerDbConnectionFactory(connectionString), CustomDbProfiler.Current);
        }
    }
}