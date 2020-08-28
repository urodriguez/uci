namespace Application.Dtos
{
    public class ResetPasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}