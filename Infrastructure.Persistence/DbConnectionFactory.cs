﻿using System.Configuration;
using System.Data;
using MiniProfiler.Integrations;

namespace Infrastructure.Persistence
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetSqlConnection()
        {
            CustomDbProfiler.Current.ProfilerContext.Reset();
            return ProfiledDbConnectionFactory.New(new SqlServerDbConnectionFactory(ConfigurationManager.ConnectionStrings["InventappContext"].ConnectionString), CustomDbProfiler.Current);
        }
    }
}