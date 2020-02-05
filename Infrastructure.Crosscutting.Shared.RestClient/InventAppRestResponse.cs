using System.Net;

namespace Infrastructure.Crosscutting.Shared.RestClient
{
    public class InventAppRestResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        
        public bool IsSuccessful() => StatusCode == HttpStatusCode.OK;
    }

    public class InventAppRestResponse<T> where T : new()
    {
        public string Content { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        
        public bool IsSuccessful() => StatusCode == HttpStatusCode.OK;
    }
}