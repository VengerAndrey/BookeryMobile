using System;

namespace BookeryMobile.Exceptions
{
    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialException() : base("Invalid email or password.")
        {
        }
    }
}