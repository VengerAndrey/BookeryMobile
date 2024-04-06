using System;

namespace BookeryMobile.Exceptions
{
    public class NameConflictException : Exception
    {
        public NameConflictException() : base("Choose a different name.")
        {
            
        }
    }
}