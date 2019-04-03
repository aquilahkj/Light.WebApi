using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Light.WebApi.Core
{
    internal class AuthorizeManagement : IAuthorizeManagement
    {
        const string USER_PREFIX = "UP";
        const string USER_ACTION_TOKEN = "UAT";

        private readonly ICacheAgent cache;
        private readonly IEncryptor encryptor;
        private readonly TimeSpan expiryTime;
        private readonly bool testMode;
        readonly IAuthorizeData authorizeData;

        public AuthorizeManagement(AuthorizeOptions options, IAuthorizeData authorizeData)
        {
            this.authorizeData = authorizeData;
            if (options.CacheType == 1) {
                cache = new RedisCacheAgent(options.RedisConfig);
            }
            else {
                cache = new MemoryCacheAgent();
            }

            encryptor = new Encryptor(options.TokenKey);
            if (options.CacheExpiry != null && options.CacheExpiry.Value > 0) {
                expiryTime = new TimeSpan(0, options.CacheExpiry.Value, 0);
            }
            else {
                expiryTime = new TimeSpan(0, 30, 0);
            }
            testMode = options.TestMode;
        }

        public bool TestMode
        {
            get {
                return testMode;
            }
        }

        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="password">Password.</param>
        public string EncryptPassword(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++) {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
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
            cache.RemoveCache(USER_PREFIX + "_" + client + "_" + userId);
        }

        public void SetAuthorize(AccountAuthorizeInfo info)
        {
            var userId = info.LoginId;
            var client = info.Client;
            var value = JsonConvert.SerializeObject(info);
            cache.SetCache($"{USER_PREFIX}_{client}_{userId}", value, expiryTime);
        }

        public AccountAuthorizeInfo GetAuthorize(TokenInfo info)
        {
            var result = cache.GetCache($"{USER_PREFIX}_{info.Client}_{info.UserId}");
            return string.IsNullOrEmpty(result) ? null : JsonConvert.DeserializeObject<AccountAuthorizeInfo>(result);
        }

        public string CreateUserToken(AccountAuthorizeInfo info)
        {
            string data = $"{USER_ACTION_TOKEN}|{info.LoginId}|{info.Client}|{info.Guid}";
            return encryptor.Encrypt(data);
        }

        public TokenInfo ParseUserToken(string token)
        {
            try {
                string data = encryptor.Decrypt(token);
                var arr = data.Split('|');
                string sign = arr[0];
                if (sign != USER_ACTION_TOKEN) {
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

        public UserInfo GetUserInfo(int userId)
        {
            return authorizeData.GetUserInfo(userId);
        }

        public string[] GetUserRoles(int userId)
        {
            return authorizeData.GetUserRoles(userId);
        }

        public string VerifyLoginUser(string account, string password, string client)
        {
            password = EncryptPassword(password);
            var user = authorizeData.VerifyUser(account, password);
            if (user == null) {
                throw new VerifyException(SR.AccountNotExistsOrPasswordError);
            }
            if (!authorizeData.VerifyUserClient(user.UserId, client)) {
                throw new VerifyException(SR.UserNotAllowUseThisClient);
            }
            var roles = authorizeData.GetUserRoles(user.UserId);
            var authorize = new AccountAuthorizeInfo() {
                LoginId = user.UserId.ToString(),
                Account = user.Account,
                UserName = user.UserName,
                Client = client,
                CreateTime = DateTime.Now,
                Guid = Guid.NewGuid().ToString("N"),
                Roles = roles
            };
            var token = CreateUserToken(authorize);
            SetAuthorize(authorize);
            return token;
        }

        readonly object locker = new object();

        HashSet<string> roleAction;

        Dictionary<string, List<RolePermission>> rolePermission;

        void InitialPermissionData()
        {
            if (rolePermission == null) {
                lock (locker) {
                    if (rolePermission == null) {
                        var mydict = new Dictionary<string, List<RolePermission>>();
                        var myhash = new HashSet<string>();
                        var array = authorizeData.GetRolePermissions();
                        foreach (var item in array) {
                            if (!mydict.TryGetValue(item.Role, out List<RolePermission> list)) {
                                list = new List<RolePermission>();
                                mydict.Add(item.Role, list);
                            }
                            list.Add(item);
                            myhash.Add($"{item.Role}_{item.Action}");
                        }
                        rolePermission = mydict;
                        roleAction = myhash;
                    }
                }
            }
        }

        public bool ValidRoleAuthorize(string role, string action)
        {
            InitialPermissionData();
            return roleAction.Contains($"{role}_{action}");
        }

        public string[] GetUserPermission(string[] roles)
        {
            InitialPermissionData();
            var permissions = new HashSet<string>();
            if (roles != null && roles.Length > 0) {
                foreach (var role in roles) {
                    if (rolePermission.TryGetValue(role, out List<RolePermission> list)) {
                        foreach (var item in list) {
                            permissions.Add(item.PermissionCode);
                        }
                    }
                }
            }
            var array = new string[permissions.Count];
            permissions.CopyTo(array);
            return array;
        }

        public void ResetPermission()
        {
            lock (locker) {
                rolePermission = null;
            }
        }


    }
}
