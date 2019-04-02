namespace Light.WebApi
{
    internal interface IPermissionManagement
    {
        bool ValidUserAuthorize(string role, string action);

        void SetRolePermissions(RolePermission[] rolePermissions);
    }
}