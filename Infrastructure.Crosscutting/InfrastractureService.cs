using Infrastructure.Crosscutting.Logging;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Infrastructure.Queueing;
using Application.Contracts.Infrastructure.Queueing.Enqueue;

namespace Infrastructure.Crosscutting
{
    public abstract class InfrastractureService
    {
        protected ILogService _logService;
        protected IRestClient _restClient;
        private readonly IEnqueueService _queueService;

        protected InfrastractureService(ILogService logService, IEnqueueService queueService)
        {
            _logService = logService;
            _queueService = queueService;
        }        
        
        protected InfrastractureService(IEnqueueService queueService)
        {
            _queueService = queueService;
        }

        public void UseLogger(ILogService logService)
        {
            _logService = logService;
        }        
        
        public void UseBaseUrl(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public void ExecuteAsync(string resource, Method httpMethod = Method.GET, IQueueable queueable = null)
        {
            if (_logService == null || _restClient == null) return;

            var childServiceName = GetType().Name; //get name from concrete instanced class
            var childMethodName = new StackTrace().GetFrame(1).GetMethod().Name; //0: current method, 1: previous method

            Task.Run(() =>
            {
                try
                {
                    var request = new RestRequest
                    {
                        Resource = resource,
                        Method = httpMethod
                    };
                    if (httpMethod == Method.POST || httpMethod == Method.PUT)
                        request.AddJsonBody(queueable);

                    if (!IsLogService(childServiceName))
                        _logService.LogInfoMessageAsync($"{childServiceName}.{childMethodName} | Sending data to {childServiceName} Micro-service");

                    var response = _restClient.Execute(request, request.Method);

                    if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.ServiceUnavailable)
                        Enqueue(queueable, childServiceName, childMethodName);

                    if (response.IsSuccessful)
                    {
                        if (!IsLogService(childServiceName))
                            _logService.LogInfoMessageAsync($"{childServiceName}.{childMethodName} | Data sent to {childServiceName} Micro-service | Status=SUCCESSFUL");
                    }
                    else
                    {
                        var notSuccessMsgToLog = $"{childServiceName}.{childMethodName} | Data sent to {childServiceName} Micro-service | Status=NOT_SUCCESSFUL - Reason={response.Content}";
                        if (IsLogService(childServiceName))
                            _logService.FileSystemLog(notSuccessMsgToLog);//only log on file system when it wasn't successful
                        else
                            _logService.LogInfoMessageAsync(notSuccessMsgToLog);
                    }
                }
                catch (Exception ex)
                {
                    var errorMsgToLog = $"{childServiceName}.{childMethodName} | An exception has occurred on {childServiceName}.{childMethodName} | errorMessage ={ex.Message}";
                    if (IsLogService(childServiceName))
                        _logService.FileSystemLog(errorMsgToLog);
                    else
                        _logService.LogErrorMessageAsync(errorMsgToLog);

                    if (queueable != null)
                        Enqueue(queueable, childServiceName, childMethodName);

                    //do not throw the exception in order to avoid finish the main request
                }
            });
        }

        //LogService can't log because it would produce a infinite recursive call
        private static bool IsLogService(string serviceName) => serviceName.Equals(typeof(LogService).Name);

        private void Enqueue(IQueueable queueable, string serviceName, string methodName)
        {
            try
            {
                _queueService.Execute(queueable, queueable.QueueItemType);
            }
            catch (Exception qex)
            {
                var queueErrorMsgToLog = $"{serviceName}.{methodName} | An exception has occurred on {serviceName} trying to enqueue data | errorMessage ={qex.Message}";
                if (IsLogService(serviceName))
                    _logService.FileSystemLog(queueErrorMsgToLog);
                else
                    _logService.LogErrorMessageAsync(queueErrorMsgToLog);
            }
        }
    }
}