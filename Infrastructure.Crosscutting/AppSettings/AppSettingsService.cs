using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Infrastructure.Crosscutting.AppSettings
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly string _baseInfrastructureApiUrl;
        private readonly string _baseInventAppApiUrl;

        public AppSettingsService()
        {
            const int inventAppApiPort = 8080;
            const int infrastructureApiPort = 8081;
            const string sqlServerInventAppDatabase = "UciRod.Inventapp";
            string sqlServerHangfireDatabase = $"{sqlServerInventAppDatabase}.Hangfire";
            const string sqlServerUser = "inventappUser";
            const string sqlServerPassword = "Uc1R0d-1nv3nt4pp";
            const string multipleactiveresultsetsTrue = "MultipleActiveResultSets=True";
            const string integratedSecuritySspi = "Integrated Security=SSPI";

            switch (Environment.Name)
            {
                case "DEV":
                {
                    const string sqlServerInstance = "localhost";
                    _baseInfrastructureApiUrl = $"www.ucirod.infrastructure-test.com:{infrastructureApiPort}";
                    _baseInventAppApiUrl = $"www.ucirod.inventapp-dev.com:{inventAppApiPort}";
                    ConnectionString = $"Server={sqlServerInstance};Database={sqlServerInventAppDatabase};User ID={sqlServerUser};Password={sqlServerPassword};{multipleactiveresultsetsTrue}";
                    DefaultTokenExpiresTime = 120;
                    HangfireInventAppConnectionString = $"Server={sqlServerInstance};Database={sqlServerHangfireDatabase};{integratedSecuritySspi}";

                    break;
                }    
                
                case "TEST":
                {
                    const string sqlServerInstance = "localhost";
                    _baseInfrastructureApiUrl = $"www.ucirod.infrastructure-test.com:{infrastructureApiPort}";
                    _baseInventAppApiUrl = $"www.ucirod.inventapp-test.com:{inventAppApiPort}";
                    ConnectionString = $"Server={sqlServerInstance};Database={sqlServerInventAppDatabase}-Test;User ID={sqlServerUser};Password={sqlServerPassword};{multipleactiveresultsetsTrue}";
                    DefaultTokenExpiresTime = 30;
                    HangfireInventAppConnectionString = $"Server={sqlServerInstance};Database={sqlServerHangfireDatabase}-Test;{integratedSecuritySspi}";

                    break;
                }         
                
                case "STAGE":
                {
                    const string sqlServerInstance = "ucirod-stage1234.amazonaws.com";
                    _baseInfrastructureApiUrl = $"www.ucirod.infrastructure-stage.com:{infrastructureApiPort}";
                    _baseInventAppApiUrl = $"www.ucirod.inventapp-stage.com:{inventAppApiPort}";
                    ConnectionString = $"Server={sqlServerInstance};Database={sqlServerInventAppDatabase};User ID={sqlServerUser};Password={sqlServerPassword};{multipleactiveresultsetsTrue}";
                    DefaultTokenExpiresTime = 30;
                    HangfireInventAppConnectionString = $"Server={sqlServerInstance};Database={sqlServerHangfireDatabase};{integratedSecuritySspi}";
                        
                    break;
                }      
                
                case "PROD":
                {
                    const string sqlServerInstance = "ucirod-prod1234.amazonaws.com";
                    _baseInfrastructureApiUrl = $"www.ucirod.infrastructure.com:{infrastructureApiPort}";
                    _baseInventAppApiUrl = $"www.ucirod.inventapp.com:{inventAppApiPort}";
                    ConnectionString = $"Server={sqlServerInstance};Database={sqlServerInventAppDatabase};User ID={sqlServerUser};Password={sqlServerPassword};{multipleactiveresultsetsTrue}";
                    DefaultTokenExpiresTime = 30;
                    HangfireInventAppConnectionString = $"Server={sqlServerInstance};Database={sqlServerHangfireDatabase};{integratedSecuritySspi}";
                        
                    break;
                }

                default: throw new ArgumentOutOfRangeException($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Invalid Environment");
            }
        }


        public string AssetsDirectory => $"{InventAppDirectory}\\Assets";

        public string AuditingApiUrlV1 => $"http://{_baseInfrastructureApiUrl}/auditing/api/v1.0";

        public string AuthenticationApiUrlV1 => $"http://{_baseInfrastructureApiUrl}/authentication/api/v1.0";

        public string ClientUrl => $"http://{_baseInventAppApiUrl}/webapi/swagger";

        public string ConnectionString { get; }

        public int DefaultTokenExpiresTime { get; }

        public InventAppEnvironment Environment => new InventAppEnvironment
        {
            Name = ConfigurationManager.AppSettings["Environment"]
        };

        public string FileSystemLogsDirectory => $"{InventAppDirectory}\\FileSystemLogs";

        public string HangfireInventAppConnectionString { get; }

        public InfrastructureCredential InfrastructureCredential => new InfrastructureCredential { Id = "InventApp", SecretKey = "1nfr4structur3_1nv3nt4pp" };

        public string InventAppDirectory => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".."));

        public string LoggingApiUrlV1 => $"http://{_baseInfrastructureApiUrl}/logging/api/v1.0";

        public string MailingApiUrlV1 => $"http://{_baseInfrastructureApiUrl}/mailing/api/v1.0";

        public string ReportingApiUrlV1 => $"http://{_baseInfrastructureApiUrl}/reporting/api/v1.0";

        public string ReportsDirectory => $"{InventAppDirectory}\\Reports";

        public string TemplatesDirectory => $"{AssetsDirectory}\\Templates";

        public string WebApiUrl => $"http://{_baseInventAppApiUrl}/webapi/api";
    }
}