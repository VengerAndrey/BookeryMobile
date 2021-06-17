using System;

namespace BookeryApi.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string data) : base($"{data} not found.")
        {
        }
    }
}