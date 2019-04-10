using System;
namespace Light.WebApi.Core
{
    /// <summary>
    /// Permission info model.
    /// </summary>
    public class PermissionInfoModel
    {
        /// <summary>
        /// User Permissions
        /// </summary>
        /// <value>The roles.</value>
        public string[] Permissions { get; set; }
    }
}
