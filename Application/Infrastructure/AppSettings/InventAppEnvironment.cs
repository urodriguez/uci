namespace Application.Infrastructure.AppSettings
{
    public class InventAppEnvironment
    {
        public string Name { get; set; }

        public bool IsDev()
        {
            return Name == "DEV";
        }
    }
}