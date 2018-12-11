using System;

namespace Light.WebApi
{
    public class ValidateException : Exception
    {
        static string CombineMessage(string name, string reason)
        {
            if (string.IsNullOrWhiteSpace(name)) {
                return reason;
            }
            int index = name.LastIndexOf('.');
            if (index > -1 & index < name.Length - 1) {
                name = name.Substring(index);
            }
            return string.Concat(name, ":", reason);
        }

        public ValidateException(string name, string reason) : base(CombineMessage(name, reason))
        {
        }
    }
}