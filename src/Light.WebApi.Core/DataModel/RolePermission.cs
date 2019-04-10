namespace Light.WebApi.Core
{
    /// <summary>
    /// Role permission.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the permission code.
        /// </summary>
        /// <value>The permission code.</value>
        public string PermissionCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the permission.
        /// </summary>
        /// <value>The name of the permission.</value>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }
    }
}