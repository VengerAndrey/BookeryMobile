using System;
using System.Net.Http;

namespace BookeryApi.Services.Common
{
    public class CustomHttpClient : HttpClient
    {
        private const string Protocol = "http";
        private const string Host = "23.102.52.70";
        private const int Port = 5100;
        public CustomHttpClient(string endpoint = "")
        {
            BaseAddress = new Uri($"{Protocol}://{Host}:{Port}/" + endpoint.Trim('/') + '/');
        }
    }
}