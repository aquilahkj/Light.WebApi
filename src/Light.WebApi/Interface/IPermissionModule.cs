namespace Light.WebApi
{
    public interface IPermissionModule
    {
        RolePermission[] GetRolePermissionInfos();
    }
}