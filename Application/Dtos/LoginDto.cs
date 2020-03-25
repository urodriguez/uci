namespace Application.Dtos
{
    public class LoginDto
    {
        public LoginStatus Status { get; set; }
        public string SecurityToken { get; set; }
    }
}