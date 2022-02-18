using System;
using System.Net.Http;

namespace BookeryApi.Services.Common
{
    public class CustomHttpClient : HttpClient
    {
        public CustomHttpClient(string endpoint = "")
        {
            BaseAddress = new Uri("http://40.113.7.124:5100/" + endpoint.Trim('/') + '/');
        }
    }
}