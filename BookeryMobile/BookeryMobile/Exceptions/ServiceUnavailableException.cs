using System;

namespace BookeryMobile.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() : base("Remote service is not available.")
        {
            
        }
    }
}