using System;

namespace Light.WebApi.Core
{
    public sealed class PermissionException : Exception
    {
        readonly string account;

        public string Account {
            get {
                return account;
            }
        }

        readonly string action;

        public string Action {
            get {
                return action;
            }
        }

        public PermissionException(string message, string account, string action) : base(message)
        {
            this.action = account;
            this.action = action;
        }
    }
}