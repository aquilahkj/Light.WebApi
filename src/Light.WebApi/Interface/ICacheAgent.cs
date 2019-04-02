using System;

namespace Light.WebApi
{
    internal interface ICacheAgent
    {
        string GetCache(string key);

        void SetCache(string key, string value, TimeSpan expiry);

        void RemoveCache(string key);
    }
}