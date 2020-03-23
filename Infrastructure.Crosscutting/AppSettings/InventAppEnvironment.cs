namespace Infrastructure.Crosscutting.AppSettings
{
    public class InventAppEnvironment
    {
        public string Name { get; set; }
        public bool IsLocal()
        {
            return Name == "DEV";
        }
    }
}