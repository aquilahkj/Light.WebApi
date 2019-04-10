using System;
namespace Light.WebApi.Core
{
    public class ParameterException : Exception
    {
        public ParameterException()
        {
        }

        public ParameterException(string message) : base(message)
        {
        }
    }
}
