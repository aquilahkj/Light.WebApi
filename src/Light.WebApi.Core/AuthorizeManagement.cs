using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Light.WebApi.Core
{
    internal class AuthorizeManagement : IAuthorizeManagement
    {
        const string UAER_PREFIX = "UP";
        const string UAER_ACTION_PREFIX = "UAP";

        private readonly ICacheAgent cache;
        private readonly IEncryptor encryptor;
        private readonly TimeSpan expiryTime;
        private readonly bool testMode;

        public AuthorizeManagement(AuthorizeOptions options)
        {
            cache = options.CacheAgent;
            encryptor = options.Encryptor;
            if (options.CacheExpiry != null && options.CacheExpiry.Value > 0) {
                expiryTime = new TimeSpan(0, options.CacheExpiry.Value, 0);
            }
            else {
                expiryTime = new TimeSpan(0, 30, 0);
            }
            testMode = options.TestMode;
        }

        public bool TestMode {
            get {
                return testMode;
            }
        }

        public string GetSystemClientId(string token)
        {
            try {
                string data = encryptor.Decrypt(token);
                var arr = data.Split('|');
                string sign = arr[0];
                if (sign != "SAT") {
                    return null;
                }
                var client = arr[1];
                return client;
            }
            catch {
                return null;
            }
        }

        public void RemoveAuthorize(string client, string userId)
        {
            cache.RemoveCache(UAER_PREFIX + "_" + client + "_" + userId);
        }

        public void SetAuthorize(AccountAuthorizeInfo info)
        {
            var userId = info.LoginId;
            var client = info.Client;
            var value = JsonConvert.SerializeObject(info);
            cache.SetCache(UAER_PREFIX + "_" + client + "_" + userId, value, expiryTime);
        }

        public AccountAuthorizeInfo GetAuthorize(TokenInfo info)
        {
            var result = cache.GetCache(UAER_PREFIX + "_" + info.Client + "_" + info.UserId);
            return string.IsNullOrEmpty(result) ? null : JsonConvert.DeserializeObject<AccountAuthorizeInfo>(result);
        }

        public string CreateUserToken(AccountAuthorizeInfo info)
        {
            string data = $"UAT|{info.LoginId}|{info.Client}|{info.Guid}";
            return encryptor.Encrypt(data);
        }

        public TokenInfo ParseUserToken(string token)
        {
            try {
                string data = encryptor.Decrypt(token);
                var arr = data.Split('|');
                string sign = arr[0];
                if (sign != "UAT") {
                    return null;
                }
                var userId = arr[1];
                var client = arr[2];
                var guid = arr[3];
                var tokenInfo = new TokenInfo(userId, client, guid);
                return tokenInfo;
            }
            catch {
                return null;
            }
        }
    }
}
