using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Contracts.Infrastructure.Crosscutting;
using Domain.Contracts.Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Shared.RestClient;

namespace Infrastructure.Crosscutting.Mailing
{
    public class EmailService : IEmailService
    {
        private readonly ILogService _logService;
        private readonly IInventAppRestClient _restClient;

        public EmailService(ILogService logService)
        {
            _logService = logService;

            const string project = "mailing";

            var envUrl = new Dictionary<string, string>
            {
                { "DEV",   $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "TEST",  $"http://www.ucirod.infrastructure-test.com:40000/{project}/api" },
                { "STAGE", $"http://www.ucirod.infrastructure-stage.com:40000/{project}/api" },
                { "PROD",  $"http://www.ucirod.infrastructure.com:40000/{project}/api" }
            };

            _restClient = new InventAppRestClient(envUrl[ConfigurationManager.AppSettings["Environment"]]);
        }

        public void Send(IEmail email)
        {
            var methodName = MethodBase.GetCurrentMethod().Name;//Get method name outside task context

            var task = new Task(() =>
            {
                try
                {
                    var request = new InventAppRestRequest
                    {
                        Resource = "emails",
                        Method = InventAppRestMethod.POST,
                        JsonBody = email
                    };

                    _logService.LogInfoMessage($"{GetType().Name}.{methodName} | Sending email data to Email Micro-service");

                    var response = _restClient.Post(request);

                    _logService.LogInfoMessage(
                        response.IsSuccessful()
                            ? $"{GetType().Name}.{methodName} | Email data sent to Email Micro-service | Status=OK"
                            : $"{GetType().Name}.{methodName} | Error sending email data to Email Micro-service | Status=FAIL - Reason={response.Content}"
                    );
                }
                catch (Exception ex)
                {
                    _logService.LogErrorMessage(
                        $"{GetType().Name}.{methodName} | " +
                        $"An exception has occurred | " +
                        $"email.to={email.To} - ex.Message={ex.Message} - ex.FullStackTrace={ex}"
                    );
                }
            });

            task.Start();
        }
    }
}