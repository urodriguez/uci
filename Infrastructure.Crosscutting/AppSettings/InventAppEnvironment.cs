namespace Infrastructure.Crosscutting.AppSettings
{
    public class InventAppEnvironment
    {
        public string Name { get; set; }
        public bool IsLocal() => Name == "DEV";

        public bool IsTest() => Name == "TEST";
    }
}