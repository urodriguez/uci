using System;
using System.Configuration;
using System.Reflection;

namespace Infrastructure.Crosscutting.AppSettings
{
    public class AppSettingsService : IAppSettingsService
    {
        public string AuditingApiUrlV1
        {
            get
            {
                const string project = "auditing";
                const string version = "1.0";

                switch (Environment.Name)
                {
                    case "DEV":   return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "TEST":  return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "STAGE": return $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api/v{version}";
                    case "PROD":  return $"http://www.ucirod.infrastructure.com:40000/{project}/api/v{version}";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public string AuthenticationApiUrlV1
        {
            get
            {
                const string project = "authentication";
                const string version = "1.0";

                switch (Environment.Name)
                {
                    case "DEV":   return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "TEST":  return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "STAGE": return $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api/v{version}";
                    case "PROD":  return $"http://www.ucirod.infrastructure.com:40000/{project}/api/v{version}";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public string ClientUrl
        {
            get
            {
                const string project = "WebApi";

                switch (Environment.Name)
                {
                    case "DEV":   return $"http://localhost:8080/{project}/swagger";
                    case "TEST":  return $"http://www.ucirod.inventapp-test.com:8083/{project}/swagger";
                    case "STAGE": return $"http://www.ucirod.inventapp-stage.com:8083/{project}/swagger";
                    case "PROD":  return $"http://www.ucirod.inventapp.com:8083/{project}/swagger";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public string ConnectionString
        {
            get
            {
                switch (Environment.Name)
                {
                    case "DEV":   return "Server=localhost;Database=UciRod.Inventapp;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True";
                    case "TEST":  return "Server=localhost;Database=UciRod.Inventapp-Test;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True";
                    case "STAGE": return "Server=localhost;Database=UciRod.Inventapp-Stage;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True";
                    case "PROD":  return "Server=localhost;Database=UciRod.Inventapp-Prod;User ID=inventappUser;Password=Uc1R0d-1nv3nt4pp;Trusted_Connection=True;MultipleActiveResultSets=True";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public int DefaultTokenExpiresTime
        {
            get
            {
                switch (Environment.Name)
                {
                    case "DEV":   return 120;
                    case "TEST":  return 30;
                    case "STAGE": return 30;
                    case "PROD":  return 30;

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public InventAppEnvironment Environment => new InventAppEnvironment
        {
            Name = ConfigurationManager.AppSettings["Environment"]
        };

        public string FileSystemLogsDirectory => $"{InventAppDirectory}\\FileSystemLogs";

        public string HangfireInventAppConnectionString
        {
            get
            {
                switch (Environment.Name)
                {
                    case "DEV": return   "Server=localhost;Database=UciRod.Inventapp.Hangfire;Integrated Security=SSPI;";
                    case "TEST": return  "Server=localhost;Database=UciRod.Inventapp.Hangfire-Test;Integrated Security=SSPI;";
                    case "STAGE": return "Server=localhost;Database=UciRod.Inventapp.Hangfire-Stage;Integrated Security=SSPI;";
                    case "PROD": return  "Server=localhost;Database=UciRod.Inventapp.Hangfire;Integrated Security=SSPI;";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public InfrastructureCredential InfrastructureCredential => new InfrastructureCredential { Id = "InventApp", SecretKey = "1nfr4structur3_1nv3nt4pp" };

        public string InventAppDirectory => AppContext.BaseDirectory;

        public string LoggingApiUrlV1
        {
            get
            {
                const string project = "logging";
                const string version = "1.0";

                switch (Environment.Name)
                {
                    case "DEV":   return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "TEST":  return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "STAGE": return $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api/v{version}";
                    case "PROD":  return $"http://www.ucirod.infrastructure.com:40000/{project}/api/v{version}";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public string MailingApiUrlV1
        {
            get
            {
                const string project = "mailing";
                const string version = "1.0";

                switch (Environment.Name)
                {
                    case "DEV":   return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "TEST":  return $"http://www.ucirod.infrastructure-test.com:40000/{project}/api/v{version}";
                    case "STAGE": return $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api/v{version}";
                    case "PROD":  return $"http://www.ucirod.infrastructure.com:40000/{project}/api/v{version}";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }

        public string WebApiUrl
        {
            get
            {
                const string project = "WebApi";

                switch (Environment.Name)
                {
                    case "DEV":   return $"http://localhost:8080/{project}/api";
                    case "TEST":  return $"http://www.ucirod.inventapp-test.com:8083/{project}/api";
                    case "STAGE": return $"http://www.ucirod.inventapp-stage.com:8083/{project}/api";
                    case "PROD":  return $"http://www.ucirod.inventapp.com:8083/{project}/api";

                    default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
                }
            }
        }
    }
}