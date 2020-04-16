namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        string GetCorrelationId();
        void LogTraceMessage(string messageToLog);
        void LogInfoMessage(string messageToLog);
        void LogErrorMessage(string messageToLog);
    }
}