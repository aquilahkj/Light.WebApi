using System;

namespace Light.WebApi.Core
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