using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using RestSharp;

namespace Infrastructure.Crosscutting.Authentication
{
    public class TokenService : DelegatingHandler, ITokenService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        public TokenService(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.AuthenticationUrl);
        }

        public TokenGenarateResponse Generate(TokenGenerateRequest tokenGenerateRequest)
        {
            try
            {
                var request = new RestRequest
                {
                    Resource = "tokens",
                    Method = Method.POST
                };
                tokenGenerateRequest.Account = _appSettingsService.InfrastructureCredential;
                request.AddJsonBody(tokenGenerateRequest);

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending Account-Claims data to Token Micro-service");

                var response = _restClient.Post<TokenGenarateResponse>(request);

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == 0)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Unable to connect to Token Micro-service | Status=NOT_FOUND"
                    );

                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Claims data to Token Micro-service | Status=UNAUTHORIZED"
                    );

                    return null;
                }

                if (!response.IsSuccessful)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Claims data to Token Micro-service | Status=FAIL - Reason={response.Content}"
                    );

                    return null;
                }

                _logService.LogInfoMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Account-Claims data sent to Token Micro-service | Status=OK"
                );

                return response.Data;
            }
            catch (Exception e)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return null;
            }
        }

        public TokenValidateResponse Validate(TokenValidateRequest tokenValidateRequest)
        {
            try
            {
                var request = new RestRequest
                {
                    Resource = "tokens/validate",
                    Method = Method.POST
                };
                tokenValidateRequest.Account = _appSettingsService.InfrastructureCredential;
                request.AddJsonBody(tokenValidateRequest);

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending Account-Token data to Token Micro-service");

                var response = _restClient.Post<TokenValidateResponse>(request);

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == 0)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Unable to connect to Token Micro-service | Status=NOT_FOUND"
                    );

                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Token data to Token Micro-service | Status=UNAUTHORIZED"
                    );

                    return null;
                }

                if (!response.IsSuccessful)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Error sending Account-Token data to Token Micro-service | Status=FAIL - Reason={response.Content}"
                    );

                    return null;
                }

                _logService.LogInfoMessage(
                    $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Account-Token data sent to Token Micro-service | Status=OK"
                );

                return response.Data;
            }
            catch (Exception e)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return null;
            }
        }
    }
}
