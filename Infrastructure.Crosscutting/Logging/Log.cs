using System;

namespace Infrastructure.Crosscutting.Logging
{
    internal class Log
    {
        public Log(string application, string project, Guid correlationId, string text, LogType type)
        {
            Application = application;
            Project = project;
            CorrelationId = correlationId;
            Text = text;
            Type = type;
        }

        public string Application { get; set; }
        public string Project { get; set; }
        public Guid CorrelationId { get; set; }
        public string Text { get; set; }
        public LogType Type { get; set; }
    }
}
