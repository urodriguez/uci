using Application.Contracts.Infrastructure.Mailing;

namespace Application.Infrastructure.Mailing
{
    public class Sender : ISender
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}