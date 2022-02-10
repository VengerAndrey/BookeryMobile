using System;

namespace BookeryApi.Exceptions
{
    public class NameConflictException : Exception
    {
        public NameConflictException() : base("Please, choose the different name.")
        {
            
        }
    }
}