using Infrastructure.Crosscutting.Queueing;

namespace Infrastructure.Crosscutting.Logging
{
    internal class Log : IQueueable
    {
        public Log(InfrastructureCredential credential, string application, string project, string correlationId, string text, LogType type, string environment) 
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
        public LogType Type { get; set; }
        public string Environment { get; set; }
        public QueueItemType QueueItemType { get; set; }
    }
}
