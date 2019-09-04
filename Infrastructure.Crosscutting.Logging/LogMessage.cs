namespace Infrastructure.Crosscutting.Logging
{
    internal class LogMessage
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
        public LogMessage(string message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }
    }
}
