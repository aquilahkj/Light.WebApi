using System;
using System.Collections.Generic;

namespace Light.WebApi
{
    class PermissionManagement : IPermissionManagement
    {
        HashSet<string> hash;

        public PermissionManagement()
        {

        }

        public PermissionManagement(IPermissionModule module)
        {
            var array = module.GetRolePermissionInfos();
            SetRolePermissions(array);
        }

        public void SetRolePermissions(RolePermission[] rolePermissions)
        {
            var nhash = new HashSet<string>();
            foreach (var item in rolePermissions) {
                nhash.Add(string.Concat(item.Role, "|", item.Permission));
            }
            hash = nhash;
        }

        public bool ValidUserAuthorize(string role, string action)
        {
            if (hash == null) {
                return false;
            }
            var key = string.Concat(role, "|", action);
            return hash.Contains(key);
        }
    }
}
