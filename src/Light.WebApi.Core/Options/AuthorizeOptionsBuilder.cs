using System;
namespace Light.WebApi.Core
{
    public class AuthorizeOptionsBuilder
    {
        public AuthorizeOptionsBuilder()
        {
        }

        int cacheType;

        string redisConfig;

        string tokenKey;

        int? cacheExpiry;

        Type permissionType;

        public void SetAuthroizeSettings(AuthorizeSettings settings)
        {
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }
            if (settings.CacheType == "MemoryCache") {
                cacheType = 0;
            }
            else if (settings.CacheType == "RedisCache") {
                cacheType = 1;
            }
            if (!string.IsNullOrEmpty(settings.RedisConfig)) {
                redisConfig = settings.RedisConfig;
            }
            if (!string.IsNullOrEmpty(settings.TokenKey)) {
                tokenKey = settings.TokenKey;
            }
            if (settings.CacheExpiry != null) {
                cacheExpiry = settings.CacheExpiry;
            }
        }

        public void UseRedisCache(string redisConfig)
        {
            this.cacheType = 1;
            this.redisConfig = redisConfig;
        }

        public void UseRedisCache()
        {
            this.cacheType = 1;
        }

        public void UseMemoryCache()
        {
            this.cacheType = 0;
        }

        public void SetTokenKey(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }

        public void SetCacheExpiry(int expiryTime)
        {
            this.cacheExpiry = expiryTime;
        }

        public void SetPermissionModule<T>() where T : IPermissionModule
        {
            permissionType = typeof(T);
        }

        internal AuthorizeOptions Build()
        {
            ICacheAgent cacheAgent;
            if (cacheType == 1) {
                cacheAgent = new RedisCacheAgent(redisConfig);
            }
            else {
                cacheAgent = new MemoryCacheAgent();
            }
            IEncryptor encryptor = new Encryptor(tokenKey);
            var options = new AuthorizeOptions() {
                CacheAgent = cacheAgent,
                Encryptor = encryptor,
                CacheExpiry = cacheExpiry
            };
            return options;
        }

        internal Type GetPermissionType()
        {
            return permissionType;
        }
    }
}
