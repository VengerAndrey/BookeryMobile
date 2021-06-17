using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookeryApi.Exceptions;

namespace BookeryApi.Services.User
{
    public class AccessService : IAccessService
    {
        private readonly HttpClient _httpClient;

        public AccessService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:42396/api/Access/");
        }

        public async Task<bool> AccessById(Guid id)
        {
            var response = await _httpClient.PostAsync($"{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new DataNotFoundException("Share");
        }

        public void SetBearerToken(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}