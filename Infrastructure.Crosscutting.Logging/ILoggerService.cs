namespace Crosscutting.Logging
{
    public interface ILoggerService
    {
        void Log(LogMessage logMessage);
        void QueueLogMessage(LogMessage logMessage);
        void FlushQueueLogMessages();
    }
}
