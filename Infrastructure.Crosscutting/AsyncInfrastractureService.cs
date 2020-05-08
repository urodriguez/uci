using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Queueing;
using Infrastructure.Crosscutting.Queueing.Enqueue;
using RestSharp;

namespace Infrastructure.Crosscutting
{
    public abstract class AsyncInfrastractureService
    {
        protected ILogService _logService;
        protected IRestClient _restClient;
        private readonly IEnqueueService _queueService;

        protected AsyncInfrastractureService(ILogService logService, IEnqueueService queueService)
        {
            _logService = logService;
            _queueService = queueService;
        }        
        
        protected AsyncInfrastractureService(IEnqueueService queueService)
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

        public void ExecuteAsync(string resource, IQueueable queueable)
        {
            if (_logService == null || _restClient == null) return;

            var childServiceName = GetType().Name; //get name from concrete instanced class
            var childMethodName = new StackTrace().GetFrame(1).GetMethod().Name; //0: current method, 1: previous method

            var task = new Task(() =>
            {
                try
                {
                    var request = new RestRequest
                    {
                        Resource = resource,
                        Method = Method.POST
                    };
                    request.AddJsonBody(queueable);

                    if (!IsLogService(childServiceName))
                        _logService.LogInfoMessage($"{childServiceName}.{childMethodName} | Sending data to {childServiceName} Micro-service");

                    var response = _restClient.Post(request);

                    if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.ServiceUnavailable) 
                        Enqueue(queueable, childServiceName, childMethodName);

                    if (response.IsSuccessful && !IsLogService(childServiceName))
                    {
                        _logService.LogInfoMessage($"{childServiceName}.{childMethodName} | Data sent to {childServiceName} Micro-service | Status=SUCCESSFUL");
                    }
                    else
                    {
                        var notSuccessMsgToLog = $"{childServiceName}.{childMethodName} | Data sent to {childServiceName} Micro-service | Status=NOT_SUCCESSFUL - Reason={response.Content}";
                        if (IsLogService(childServiceName))
                            _logService.FileSystemLog(notSuccessMsgToLog);//only log on file system when it wasn't successful
                        else
                            _logService.LogInfoMessage(notSuccessMsgToLog);
                    }
                }
                catch (Exception ex)
                {
                    var errorMsgToLog = $"{childServiceName}.{childMethodName} | An exception has occurred on {childServiceName}.{childMethodName} | errorMessage ={ex.Message}";
                    if (IsLogService(childServiceName))
                        _logService.FileSystemLog(errorMsgToLog);
                    else
                        _logService.LogErrorMessage(errorMsgToLog);

                    Enqueue(queueable, childServiceName, childMethodName);

                    //do not throw the exception in order to avoid finish the main request
                }
            });

            task.Start();
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
                    _logService.LogErrorMessage(queueErrorMsgToLog);
            }
        }
    }
}