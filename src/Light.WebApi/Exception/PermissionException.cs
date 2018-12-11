using System;

namespace Light.WebApi
{
    public class PermissionException : Exception
    {
        public PermissionException()
        {
        }

        public PermissionException(string message) : base(message)
        {
        }
    }
}