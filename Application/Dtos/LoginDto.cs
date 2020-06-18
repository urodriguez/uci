namespace Application.Dtos
{
    public class LoginDto
    {
        public LoginStatus Status { get; set; }
        public SecurityTokenDto SecurityToken { get; set; }
    }
}