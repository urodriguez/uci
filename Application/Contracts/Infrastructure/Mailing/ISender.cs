namespace Application.Contracts.Infrastructure.Mailing
{
    public interface ISender
    {
        string Name { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}