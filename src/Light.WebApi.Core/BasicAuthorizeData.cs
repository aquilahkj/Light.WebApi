using System;
using System.Collections.Generic;

namespace Light.WebApi.Core
{
    class BasicAuthorizeData : IAuthorizeData
    {
        readonly Dictionary<int, AdminUser> dict = new Dictionary<int, AdminUser>();

        readonly Dictionary<string, AdminUser> dict2 = new Dictionary<string, AdminUser>();

        public BasicAuthorizeData(AdminUser[] users)
        {
            foreach (var item in users) {
                dict.Add(item.UserId, item);
            }
            foreach (var item in users) {
                dict2.Add(item.Account, item);
            }
        }

        public RolePermission[] GetRolePermissions()
        {
            return new RolePermission[0];
        }

        public UserInfo GetUserInfo(int userId)
        {
            if (dict.TryGetValue(userId, out AdminUser user)) {
                var info = new UserInfo() {
                    UserId = userId,
                    Account = user.Account,
                    UserName = user.UserName,
                    Avatar = string.Empty,
                    Description = string.Empty,
                    Roles = new string[] { "admin" }
                };
                return info;
            }
            else {
                return null;
            }
        }

        public string[] GetUserRoles(int userId)
        {
            return new string[] { "admin" };
        }

        public UserInfo VerifyUser(string account, string password)
        {
            if (dict2.TryGetValue(account, out AdminUser user)) {
                if (user.Password == password) {
                    var info = new UserInfo() {
                        UserId = user.UserId,
                        Account = user.Account,
                        UserName = user.UserName,
                        Avatar = string.Empty,
                        Description = string.Empty,
                        Roles = new string[] { "admin" }
                    };
                    return info;
                }
            }
            return null;
        }

        public bool VerifyUserClient(int userId, string client)
        {
            return dict.ContainsKey(userId);
        }
    }
}
