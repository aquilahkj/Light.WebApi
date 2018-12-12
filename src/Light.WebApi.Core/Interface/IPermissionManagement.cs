namespace Light.WebApi.Core
{
    public interface IPermissionManagement
    {
        bool ValidUserAuthorize(string role, string action);

        void SetRolePermissions(RolePermission[] rolePermissions);
    }
}