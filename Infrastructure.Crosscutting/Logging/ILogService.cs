using System;

namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        Guid GetCorrelationId();
        void LogTraceMessage(string messageToLog);
        void LogInfoMessage(string messageToLog);
        void LogErrorMessage(string messageToLog);
    }
}