namespace Application.Dtos
{
    public class UserCredentialDto : IDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}