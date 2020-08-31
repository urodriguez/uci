namespace Application.Contracts.Infrastructure.Logging
{
    public interface ILogService
    {
        string GetCorrelationId();
        void LogAsync(IMessage message);
        void LogTraceMessageAsync(string messageToLog);
        void LogInfoMessageAsync(string messageToLog);
        void LogErrorMessageAsync(string messageToLog);
        void FileSystemLog(string messageToLog);
        void DeleteOldLogs();
    }
}