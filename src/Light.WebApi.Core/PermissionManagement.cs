using System;
using System.Collections.Generic;

namespace Light.WebApi.Core
{
    class PermissionManagement : IPermissionManagement
    {
        private readonly IPermissionModule module;
        HashSet<string> hash;

        public PermissionManagement()
        {

        }

        public PermissionManagement(IPermissionModule module)
        {
            var array = module.GetRolePermissionInfos();
            SetRolePermissions(array);
            this.module = module;
        }

        public void SetRolePermissions(RolePermission[] rolePermissions)
        {
            var nhash = new HashSet<string>();
            foreach (var item in rolePermissions) {
                nhash.Add(string.Concat(item.Role, "|", item.Permission));
            }
            hash = nhash;
        }

        public bool ValidRoleAuthorize(string role, string action)
        {
            if (hash == null) {
                return false;
            }
            var key = string.Concat(role, "|", action);
            return hash.Contains(key);
        }
    }
}
