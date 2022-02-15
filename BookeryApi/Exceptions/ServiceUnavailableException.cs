using System;

namespace BookeryApi.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() : base("Remote service is not available.")
        {
            
        }
    }
}