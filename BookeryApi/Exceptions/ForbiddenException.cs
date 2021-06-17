using System;

namespace BookeryApi.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("You're not an owner.")
        {
        }
    }
}