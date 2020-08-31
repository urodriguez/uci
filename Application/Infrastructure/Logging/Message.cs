using Application.Contracts.Infrastructure.Queueing;
using Application.Infrastructure.Queueing;

namespace Application.Infrastructure.Logging
{
    public class Message : IQueueable
    {
        public Message(InfrastructureCredential credential, string application, string project, string correlationId, string text, MessageType type, string environment) 
        {
            Credential = credential;
            Application = application;
            Project = project;
            CorrelationId = correlationId;
            Text = text;
            Type = type;
            Environment = environment;
            QueueItemType = QueueItemType.Log;
        }

        public InfrastructureCredential Credential { get; set; }
        public string Application { get; set; }
        public string Project { get; set; }
        public string CorrelationId { get; set; }
        public string Text { get; set; }
        public MessageType Type { get; set; }
        public string Environment { get; set; }
        public QueueItemType QueueItemType { get; set; }
    }
}
