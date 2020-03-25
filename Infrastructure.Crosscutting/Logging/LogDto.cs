using System;

namespace Infrastructure.Crosscutting.Logging
{
    internal class LogDto
    {
        public LogDto(InfrastructureAccount account, string application, string project, Guid correlationId, string text, LogType type)
        {
            Account = account;
            Application = application;
            Project = project;
            CorrelationId = correlationId;
            Text = text;
            Type = type;
        }

        public InfrastructureAccount Account { get; set; }
        public string Application { get; set; }
        public string Project { get; set; }
        public Guid CorrelationId { get; set; }
        public string Text { get; set; }
        public LogType Type { get; set; }
    }
}
