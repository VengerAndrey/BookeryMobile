using System;

namespace BookeryMobile.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base($"User does not exist.")
        {
        }
    }
}