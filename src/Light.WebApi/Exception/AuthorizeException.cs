using System;

namespace Light.WebApi
{
    public class AuthorizeException : Exception
    {
        public AuthorizeException()
        {
        }

        public AuthorizeException(string message) : base(message)
        {
        }
    }
}