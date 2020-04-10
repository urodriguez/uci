﻿using System;

namespace Infrastructure.Crosscutting.Logging
{
    public class CorrelationException : Exception
    {
        public string OriginalStackTrace { get; set; }

        public CorrelationException(string message, string originalStackTrace) : base(message)
        {
            OriginalStackTrace = originalStackTrace;
        }

        public override string StackTrace => OriginalStackTrace;
    }
}