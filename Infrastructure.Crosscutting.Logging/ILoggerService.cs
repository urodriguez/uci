namespace Infrastructure.Crosscutting.Logging
{
    public interface ILoggerService
    {
        void LogTrace(string logMessage);
        void LogError(string logMessage);
        void QueueMessageTrace(string messageToLog);
        void QueueMessageError(string messageToLog);
        void FlushQueueMessages();
    }
}
