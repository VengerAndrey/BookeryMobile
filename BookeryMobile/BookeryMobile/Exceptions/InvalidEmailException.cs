using System;

namespace BookeryMobile.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email) : base($"Email {email} is invalid.")
        {
        }
    }
}