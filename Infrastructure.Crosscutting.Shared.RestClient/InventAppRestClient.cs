using RestSharp;

namespace Infrastructure.Crosscutting.Shared.RestClient
{
    public class InventAppRestClient : IInventAppRestClient
    {
        private readonly IRestClient _restClient;

        public InventAppRestClient(string baseUrl)
        {
            _restClient = new RestSharp.RestClient(baseUrl);
        }

        public InventAppRestResponse Post(InventAppRestRequest request)
        {
            var restSharpRequest = BuildRestSharpRequest(request);

            var restSharpResponse = _restClient.Post(restSharpRequest);

            return new InventAppRestResponse
            {
                Content = restSharpResponse.Content,
                StatusCode = restSharpResponse.StatusCode
            };
        }

        public InventAppRestResponse<T> Post<T>(InventAppRestRequest request) where T : new()
        {
            var restSharpRequest = BuildRestSharpRequest(request);

            var restSharpResponse = _restClient.Post<T>(restSharpRequest);

            return new InventAppRestResponse<T>
            {
                Content = restSharpResponse.Content,
                Data = restSharpResponse.Data,
                StatusCode = restSharpResponse.StatusCode
            };
        }

        public RestRequest BuildRestSharpRequest(InventAppRestRequest inventAppRestRequest)
        {
            var restSharpRequest = new RestRequest(inventAppRestRequest.Resource, (Method)(int)inventAppRestRequest.Method);
            restSharpRequest.AddJsonBody(inventAppRestRequest.JsonBody);

            return restSharpRequest;
        }
    }
}
