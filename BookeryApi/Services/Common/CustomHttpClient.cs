using System;
using System.Net.Http;

namespace BookeryApi.Services.Common
{
    public class CustomHttpClient : HttpClient
    {
        public CustomHttpClient(string endpoint = "")
        {
            BaseAddress = new Uri("https://webapi-aq0.conveyor.cloud/api/" + endpoint.Trim('/') + '/');
        }
    }
}