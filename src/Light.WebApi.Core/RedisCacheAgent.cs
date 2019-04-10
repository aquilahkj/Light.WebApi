using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace Light.WebApi.Core
{
    class RedisCacheAgent : ICacheAgent
    {
        readonly RedisCache _client;

        public RedisCacheAgent(string redisConfig)
        {
            if (string.IsNullOrWhiteSpace(redisConfig)) {
                redisConfig = "127.0.0.1:6379,abortConnect=false";
            }
            _client = new RedisCache(new RedisCacheOptions() {
                Configuration = redisConfig
            });
        }

        public string GetCache(string key)
        {
            return _client.GetString(key);
        }

        public void RemoveCache(string key)
        {
            _client.Remove(key);
        }

        public void SetCache(string key, string value, TimeSpan expiry)
        {
            _client.SetString(key, value, new DistributedCacheEntryOptions() {
                SlidingExpiration = expiry
            });
        }
    }
}
