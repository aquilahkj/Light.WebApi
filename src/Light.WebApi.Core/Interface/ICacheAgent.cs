using System;

namespace Light.WebApi.Core
{
    public interface ICacheAgent
    {
        string GetCache(string key);

        void SetCache(string key, string value, TimeSpan expiry);

        void RemoveCache(string key);
    }
}