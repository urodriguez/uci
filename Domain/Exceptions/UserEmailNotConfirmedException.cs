namespace Domain.Exceptions
{
    public class UserEmailNotConfirmedException : BusinessRuleException
    {
        public UserEmailNotConfirmedException(string userName) : base($"User '{userName}' has not been confirmed his email yet")
        {
        }
    }
}