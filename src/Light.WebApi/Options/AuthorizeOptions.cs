using System;
namespace Light.WebApi
{
    public class AuthorizeOptions
    {
        public AuthorizeOptions()
        {
        }

        public int? CacheExpiry { get; internal set; }
        internal ICacheAgent CacheAgent { get; set; }
        internal IEncryptor Encryptor { get; set; }
    }
}
