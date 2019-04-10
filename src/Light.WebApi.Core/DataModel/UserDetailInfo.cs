using System;
namespace Light.WebApi.Core
{
    /// <summary>
    /// User informations.
    /// </summary>
    public class UserDetailInfo
    {
        /// <summary>
        /// User Account
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        /// <value>The name of the user.</value>
        public string Name { get; set; }
        /// <summary>
        /// User Avatar
        /// </summary>
        /// <value>The avatar.</value>
        public string Avatar { get; set; }
        /// <summary>
        /// User Introduction
        /// </summary>
        /// <value>The description.</value>
        public string Introduction { get; set; }
        /// <summary>
        /// User Roles
        /// </summary>
        /// <value>The roles.</value>
        public string[] Roles { get; set; }
        /// <summary>
        /// User Permissions
        /// </summary>
        /// <value>The permissions.</value>
        public string[] Permissions { get; set; }
    }
}
