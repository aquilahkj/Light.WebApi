using System;

namespace Light.WebApi.Core
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