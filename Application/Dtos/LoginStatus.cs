namespace Application.Dtos
{
    public enum LoginStatus
    {
        Success = 0,
        NonCustomPassword = 1,
        UnconfirmedEmail = 2,
        Locked = 3
    }
}