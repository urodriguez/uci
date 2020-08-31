using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Infrastructure.Redering;
using Application.Infrastructure.Redering;
using RestSharp;

namespace Infrastructure.Crosscutting.Rendering
{
    public class TemplateService : ITemplateService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        public TemplateService(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.RenderingApiUrlV1);
        }

        public async Task<T> RenderAsync<T>(Template template)
        {
            try
            {
                var request = new RestRequest
                {
                    Resource = "templates",
                    Method = Method.POST
                };

                template.Credential = _appSettingsService.InfrastructureCredential;

                var genericType = typeof(T).Name;
                template.RenderAs = genericType == "String" ? RenderAs.String : RenderAs.Bytes;

                request.AddJsonBody(template);

                _logService.LogInfoMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending template to Rendering Micro-service");

                var response = genericType == "String"
                    ? await _restClient.ExecuteAsync<T>(request, request.Method)
                    : await _restClient.ExecuteAsync(request, request.Method);

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == 0)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Unable to connect to Rendering Micro-service | Status=NOT_FOUND"
                    );
                    
                    return default(T);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending template to Rendering Micro-service | Status=UNAUTHORIZED"
                    );

                    return default(T);
                }

                if (!response.IsSuccessful)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | " +
                        $"Error sending template to Rendering Micro-service | " +
                        $"Status=FAIL - response.Content={response.Content} - response.ErrorMessage={response.ErrorMessage}"
                    );

                    return default(T);
                }

                _logService.LogInfoMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | template sent to Rendering Micro-service | Status=OK"
                );

                return genericType == "String" 
                    ? ((IRestResponse<T>)response).Data 
                    : (T)(object)response.RawBytes;
            }
            catch (Exception e)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return default(T);
            }
        }
    }
}