using System;
namespace Light.WebApi.Core
{
    class AuthorizeOptions
    {
        public int? CacheExpiry { get; set; }

        public bool TestMode { get; set; }

        public int CacheType { get; set; }

        public string RedisConfig { get; set; }

        public string TokenKey { get; set; }

        public IAuthorizeData AuthorizeData { get; set; }
    }
}
