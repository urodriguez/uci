using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.HttpActionResults
{
    public class HtmlActionResult : IHttpActionResult
    {
        private readonly string _html;
        HttpRequestMessage _request;

        public HtmlActionResult(string html, HttpRequestMessage request)
        {
            _html = html;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(_html),
                RequestMessage = _request
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return Task.FromResult(response);
        }
    }
}