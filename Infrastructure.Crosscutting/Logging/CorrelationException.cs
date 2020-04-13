using System;

namespace Infrastructure.Crosscutting.Logging
{
    public class CorrelationException : Exception
    {
        public CorrelationException(Exception inner) : base("An error has ocurred trying to generate a Correlation", inner)
        {
        }
    }
}