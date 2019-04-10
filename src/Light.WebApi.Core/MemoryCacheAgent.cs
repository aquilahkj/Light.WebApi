using System;
using Microsoft.Extensions.Caching.Memory;

namespace Light.WebApi.Core
{
    class MemoryCacheAgent : ICacheAgent
    {
        readonly MemoryCache _client;

        public MemoryCacheAgent()
        {
            _client = new MemoryCache(new MemoryCacheOptions());
        }

        public string GetCache(string key)
        {
            return _client.Get<string>(key);
        }

        public void RemoveCache(string key)
        {
            _client.Remove(key);
        }

        public void SetCache(string key, string value, TimeSpan expiry)
        {
            _client.Set<string>(key, value, new MemoryCacheEntryOptions() {
                SlidingExpiration = expiry
            });
        }
    }
}
