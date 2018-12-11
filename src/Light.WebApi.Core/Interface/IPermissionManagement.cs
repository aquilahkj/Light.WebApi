namespace Light.WebApi.Core
{
    internal interface IPermissionManagement
    {
        bool ValidUserAuthorize(string role, string action);

        void SetRolePermissions(RolePermission[] rolePermissions);
    }
}