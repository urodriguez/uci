namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        string GetCorrelationId();
        void Log(Log log);
        void LogTraceMessage(string messageToLog);
        void LogInfoMessage(string messageToLog);
        void LogErrorMessage(string messageToLog);
        void FileSystemLog(string messageToLog);
        void DeleteOldLogs();
    }
}