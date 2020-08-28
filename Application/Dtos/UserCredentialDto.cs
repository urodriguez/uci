namespace Application.Dtos
{
    public class UserCredentialDto : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}