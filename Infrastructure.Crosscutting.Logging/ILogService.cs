using System.Threading.Tasks;

namespace Infrastructure.Crosscutting.Logging
{
    public interface ILogService
    {
        void LogTraceMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void LogInfoMessage(string messageToLog, MessageType messageType = MessageType.Text);
        void LogErrorMessage(string messageToLog, MessageType messageType = MessageType.Text);
    }
}
