namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        void QueueTraceMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void QueueInfoMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void QueueErrorMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void FlushQueueMessages();
    }
}
