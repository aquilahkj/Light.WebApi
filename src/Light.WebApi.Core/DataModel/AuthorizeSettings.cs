using System;
namespace Light.WebApi.Core
{
    public class AuthorizeSettings
    {
        public string CacheType { get; set; }

        public string RedisConfig { get; set; }

        public string TokenKey { get; set; }

        public int? CacheExpiry { get; set; }
    }
}
