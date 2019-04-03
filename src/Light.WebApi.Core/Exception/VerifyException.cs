using System;
namespace Light.WebApi.Core
{
    public class VerifyException : Exception
    {
        public VerifyException()
        {
        }

        public VerifyException(string message) : base(message)
        {
        }
    }
}
