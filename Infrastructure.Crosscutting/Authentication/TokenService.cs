using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure.AppSettings;
using Application.Contracts.Infrastructure.Authentication;
using Application.Contracts.Infrastructure.Logging;
using Application.Infrastructure.Authentication;
using RestSharp;

namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        public TokenService(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.AuthenticationApiUrlV1);
        }

        public async Task<TokenGenerateResponse> GenerateAsync(TokenGenerateRequest tokenGenerateRequest)
        {
            try
            {
                var request = new RestRequest
                {
                    Resource = "tokens",
                    Method = Method.POST
                };
                tokenGenerateRequest.Credential = _appSettingsService.InfrastructureCredential;
                request.AddJsonBody(tokenGenerateRequest);

                _logService.LogInfoMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending Account-Claims data to Token Micro-service");

                var response = await _restClient.ExecuteAsync<TokenGenerateResponse>(request);

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == 0)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Unable to connect to Token Micro-service | Status=NOT_FOUND"
                    );

                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Claims data to Token Micro-service | Status=UNAUTHORIZED"
                    );

                    return null;
                }

                if (!response.IsSuccessful)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Claims data to Token Micro-service | Status=FAIL - Reason={response.Content}"
                    );

                    return null;
                }

                _logService.LogInfoMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Account-Claims data sent to Token Micro-service | Status=OK"
                );

                return response.Data;
            }
            catch (Exception e)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return null;
            }
        }

        public async Task<TokenValidateResponse> ValidateAsync(TokenValidateRequest tokenValidateRequest)
        {
            try
            {
                var request = new RestRequest
                {
                    Resource = "tokens/validate",
                    Method = Method.POST
                };
                tokenValidateRequest.Credential = _appSettingsService.InfrastructureCredential;
                request.AddJsonBody(tokenValidateRequest);

                _logService.LogInfoMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending Account-Token data to Token Micro-service");

                var response = await _restClient.ExecuteAsync<TokenValidateResponse>(request);

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == 0)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Unable to connect to Token Micro-service | Status=NOT_FOUND"
                    );

                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Token data to Token Micro-service | Status=UNAUTHORIZED"
                    );

                    return null;
                }

                if (!response.IsSuccessful)
                {
                    _logService.LogErrorMessageAsync(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Token data to Token Micro-service | Status=FAIL - Reason={response.Content}"
                    );

                    return null;
                }

                _logService.LogInfoMessageAsync(
                    $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Account-Token data sent to Token Micro-service | Status=OK"
                );

                return response.Data;
            }
            catch (Exception e)
            {
                _logService.LogErrorMessageAsync($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return null;
            }
        }
    }
}
