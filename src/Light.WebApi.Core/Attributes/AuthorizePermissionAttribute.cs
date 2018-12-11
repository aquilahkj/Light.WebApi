using System;
namespace Light.WebApi.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizePermissionAttribute : Attribute
    {
        private readonly AuthorizeType type;

        public AuthorizeType Type {
            get {
                return type;
            }
        }

        public AuthorizePermissionAttribute(AuthorizeType type)
        {
            this.type = type;
        }

        public AuthorizePermissionAttribute() : this(AuthorizeType.UserBasic)
        {

        }
    }

    public enum AuthorizeType
    {
        UserBasic = 1,
        UserAction = 2,
        System = 3
    }
}
