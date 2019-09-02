namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        void LogTrace(string logMessage);
        void LogError(string logMessage);
        void QueueTraceMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void QueueErrorMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void FlushQueueMessages();
    }
}
