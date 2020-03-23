using System;

namespace Application.Exceptions
{
    public class AuthenticationFailException : Exception
    {
        public AuthenticationFailException(string message = "") : base(message)
        {
        }
    }
}