namespace Light.WebApi.Core
{
    public interface IPermissionModule
    {
        RolePermission[] GetRolePermissionInfos();
    }
}