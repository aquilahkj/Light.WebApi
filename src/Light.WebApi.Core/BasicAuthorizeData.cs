using System;
using System.Collections.Generic;

namespace Light.WebApi.Core
{
    public class BasicAuthorizeData : IAuthorizeData
    {
        Dictionary<int, AdminUser> dict = new Dictionary<int, AdminUser>();

        public BasicAuthorizeData(AdminUser[] users)
        {
            foreach (var item in users) {
                dict[item.UserId] = item;
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
                    Avatar = string.Empty
                };
                return info;
            }
            else {
                return null;
            }
        }

        public string[] GetUserRoles(int userId)
        {
            if (dict.ContainsKey(userId)) {
                return new string[] { "admin" };
            }
            else {
                return new string[0];
            }
        }

        public UserInfo VerifyUser(string account, string password)
        {
            throw new NotImplementedException();
        }

        public bool VerifyUserClient(int userId, string client)
        {
            return dict.ContainsKey(userId);
        }
    }
}
