namespace Infrastructure.Crosscutting.Shared.RestClient
{
    public interface IInventAppRestClient
    {
        InventAppRestResponse Post(InventAppRestRequest request);
        InventAppRestResponse<T> Post<T>(InventAppRestRequest request) where T : new();
    }
}