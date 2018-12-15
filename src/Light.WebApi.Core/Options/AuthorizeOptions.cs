using System;
namespace Light.WebApi.Core
{
    public class AuthorizeOptions
    {
        public AuthorizeOptions()
        {
        }

        public int? CacheExpiry { get; internal set; }
        public ICacheAgent CacheAgent { get; set; }
        public IEncryptor Encryptor { get; set; }
        public bool TestMode { get; set; }
    }
}
