using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BookeryMobile.Data;

namespace BookeryMobile.Services.Common
{
    public class BaseService : IBaseService
    {
        protected readonly HttpClient HttpClient;

        protected BaseService(string serviceUrl = "")
        {
            var baseUrl = AppConfiguration.Instance["BaseUrl"];
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.Trim('/') + '/'  + serviceUrl.Trim('/') + '/')
            };
        }

        public void SetBearerToken(string accessToken)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}