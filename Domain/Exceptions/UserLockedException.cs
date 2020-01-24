namespace Domain.Exceptions
{
    public class UserLockedException : BusinessRuleException
    {
        public UserLockedException(string userName) : base($"User '{userName}' has his account locked")
        {
        }
    }
}