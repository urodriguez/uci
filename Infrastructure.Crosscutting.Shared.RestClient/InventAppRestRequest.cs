namespace Infrastructure.Crosscutting.Shared.RestClient
{
    public class InventAppRestRequest
    {
        public string Resource { get; set; }
        public InventAppRestMethod Method { get; set; }
        public object JsonBody { get; set; }
    }
}