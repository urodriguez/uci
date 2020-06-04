using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using RestSharp;

namespace Infrastructure.Crosscutting.Reporting
{
    public class ReportInfrastructureService : IReportInfrastructureService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        public ReportInfrastructureService(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.ReportingApiUrlV1);
        }

        public async Task<byte[]> CreateAsync(Report report)
        {
            try
            {
                var request = new RestRequest
                {
                    Resource = "reports",
                    Method = Method.POST
                };
                report.Credential = _appSettingsService.InfrastructureCredential;
                request.AddJsonBody(report);

                _logService.LogInfoMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending report to Report Micro-service");

                var response = await _restClient.ExecuteAsync(request, request.Method);

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == 0)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Unable to connect to Report Micro-service | Status=NOT_FOUND"
                    );

                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending report to Report Micro-service | Status=UNAUTHORIZED"
                    );

                    return null;
                }

                if (!response.IsSuccessful)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending report to Report Micro-service | Status=FAIL - Reason={response.Content}"
                    );

                    return null;
                }

                _logService.LogInfoMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | report sent to Report Micro-service | Status=OK"
                );

                return response.RawBytes;
            }
            catch (Exception e)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return null;
            }
        }
    }
}