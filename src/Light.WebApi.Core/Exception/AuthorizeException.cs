using System;

namespace Light.WebApi.Core
{
    public sealed class AuthorizeException : Exception
    {
        readonly string token;

        public string Token {
            get {
                return token;
            }
        }

        public AuthorizeException(string message, string token) : base(message)
        {
            this.token = token;
        }
    }
}