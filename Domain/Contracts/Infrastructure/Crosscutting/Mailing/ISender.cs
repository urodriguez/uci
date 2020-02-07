namespace Domain.Contracts.Infrastructure.Crosscutting.Mailing
{
    public interface ISender
    {
        string Name { get; }
        string Email { get; }
        string Password { get; }

    }
}