using Domain.Contracts.Infrastructure.Crosscutting.Mailing;

namespace Infrastructure.Crosscutting.Mailing
{
    public class Host : IHost
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}