namespace Infrastructure.Crosscutting.Reporting
{
    public class Report
    {
        public InfrastructureCredential Credential { get; set; }
        public string Template { get; set; }
        public string Data { get; set; }
    }
}