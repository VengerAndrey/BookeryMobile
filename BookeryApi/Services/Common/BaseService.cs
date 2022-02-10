using System.Net.Http;
using System.Net.Http.Headers;

namespace BookeryApi.Services.Common
{
    public class BaseService : IBaseService
    {
        protected HttpClient _httpClient;

        public BaseService(string endpoint = "")
        {
            _httpClient = new CustomHttpClient(endpoint);
        }

        public void SetBearerToken(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}