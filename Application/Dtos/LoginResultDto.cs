namespace Application.Dtos
{
    public class LoginResultDto
    {
        public LoginStatus Status { get; set; }
        public SecurityTokenDto SecurityToken { get; set; }
    }
}