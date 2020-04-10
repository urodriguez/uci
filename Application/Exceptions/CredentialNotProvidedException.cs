using System;

namespace Application.Exceptions
{
    public class CredentialNotProvidedException : Exception
    {
        public CredentialNotProvidedException() : base("Credential not provided")
        {
            
        }
    }
}