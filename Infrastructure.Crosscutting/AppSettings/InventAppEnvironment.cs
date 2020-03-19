using Domain.Contracts.Infrastructure.Crosscutting.AppSettings;

namespace Infrastructure.Crosscutting.AppSettings
{
    public class InventAppEnvironment : IInventAppEnvironment
    {
        public string Name { get; set; }
        public bool IsLocal()
        {
            return Name == "DEV";
        }
    }
}