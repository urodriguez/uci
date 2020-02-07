using Domain.Contracts.Infrastructure.Crosscutting.Mailing;

namespace Infrastructure.Crosscutting.Mailing
{
    public class Sender : ISender
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}