namespace Light.WebApi.Core
{
    public interface IPermissionManagement
    {
        bool ValidRoleAuthorize(string role, string action);

        void SetRolePermissions(RolePermission[] rolePermissions);
    }
}