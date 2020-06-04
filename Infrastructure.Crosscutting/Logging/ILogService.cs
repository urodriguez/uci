namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        string GetCorrelationId();
        void LogAsync(Log log);
        void LogTraceMessageAsync(string messageToLog);
        void LogInfoMessageAsync(string messageToLog);
        void LogErrorMessageAsync(string messageToLog);
        void FileSystemLog(string messageToLog);
        void DeleteOldLogs();
    }
}