using Application.Contracts.Infrastructure.Mailing;

namespace Application.Infrastructure.Mailing
{
    public class Host : IHost
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}