namespace Application.Dtos
{
    public enum LoginStatus
    {
        Success = 0,
        InvalidPassword = 1,
        NonCustomPassword = 2,
        UnconfirmedEmail = 3,
        Locked = 4,
        Inactive = 5
    }
}