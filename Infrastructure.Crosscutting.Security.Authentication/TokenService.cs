using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using Domain.Contracts.Infrastructure.Crosscutting;
using RestSharp;

namespace Infrastructure.Crosscutting.Security.Authentication
{
    public class TokenService : DelegatingHandler, ITokenService
    {
        private const string AccountId = "InventApp";
        private const string AccountSecret = "1nfr4structur3_1nv3nt4pp";

        private readonly ILogService _logService;
        private readonly IRestClient _restClient;

        public TokenService(ILogService logService)
        {
            _logService = logService;

            const string project = "authentication";

            var envUrl = new Dictionary<string, string>
            {
                { "DEV",   $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "TEST",  $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "STAGE", $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api" },
                { "PROD",  $"http://www.ucirod.infrastructure.com:40000/{project}/api" }
            };

            _restClient = new RestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);
        }

        public ISecurityToken Generate(IReadOnlyCollection<Claim> claims)
        {
            try
            {
                var request = new RestRequest("tokens", Method.POST);
                request.AddJsonBody(new
                {
                    Account = new { Id = AccountId, Secret = AccountSecret },
                    claims
                });

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending Account-Claims data to Token Micro-service");

                var response = _restClient.Post<SecurityToken>(request);

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

        public IEnumerable<Claim> Validate(string securityToken)
        {
            try
            {
                var request = new RestRequest("tokens/validate", Method.POST);
                request.AddJsonBody(new
                {
                    Account = new { Id = AccountId, Secret = AccountSecret },
                    Token = securityToken
                });

                _logService.LogInfoMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | Sending Account-Token data to Token Micro-service");

                var response = _restClient.Post<TokenValidateDto>(request);

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

                return response.Data.Claims.Select(d => new Claim(d.Type, d.Value));
            }
            catch (Exception e)
            {
                _logService.LogErrorMessage($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} | e.Message={e.Message} - e={e}");

                return null;
            }
        }
    }
}
