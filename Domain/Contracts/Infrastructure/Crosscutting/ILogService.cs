namespace Domain.Contracts.Infrastructure.Crosscutting
{
    public interface ILogService
    {
        void LogTraceMessage(string messageToLog);
        void LogInfoMessage(string messageToLog);
        void LogErrorMessage(string messageToLog);
    }
}