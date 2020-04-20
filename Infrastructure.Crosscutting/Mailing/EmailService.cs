using System;
using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;
using RestSharp;

namespace Infrastructure.Crosscutting.Mailing
{
    public class EmailService : IEmailService
    {
        private readonly ILogService _logService;
        private readonly IRestClient _restClient;
        private readonly IAppSettingsService _appSettingsService;

        public EmailService(ILogService logService, IAppSettingsService appSettingsService)
        {
            _logService = logService;
            _appSettingsService = appSettingsService;
            _restClient = new RestClient(appSettingsService.MailingApiUrlV1);
        }

        public void Send(Email email)
        {
            var methodName = MethodBase.GetCurrentMethod().Name;//Get method name outside task context

            var task = new Task(() =>
            {
                try
                {
                    var request = new RestRequest
                    {
                        Resource = "emails",
                        Method = Method.POST
                    };
                    email.Credential = _appSettingsService.InfrastructureCredential;
                    request.AddJsonBody(email);

                    _logService.LogInfoMessage($"{GetType().Name}.{methodName} | Sending email data to Email Micro-service");

                    var response = _restClient.Post(request);

                    _logService.LogInfoMessage(
                        response.IsSuccessful
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