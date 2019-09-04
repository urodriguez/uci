namespace Infrastructure.Crosscutting.Logging
{
    internal class LogMessage
    {
        public string Message { get; set; }
        public LogType LogType { get; set; }
        public LogMessage(string message, LogType logType)
        {
            Message = message;
            LogType = logType;
        }
    }
}
