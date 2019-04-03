using System;
namespace Light.WebApi.Core
{
    public interface IAuthorizeData
    {
        /// <summary>
        /// Gets the user info by identifier.
        /// </summary>
        /// <returns>The user info by identifier.</returns>
        /// <param name="userId">User identifier.</param>
        UserInfo GetUserInfo(int userId);
        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <returns>The user roles.</returns>
        /// <param name="userId">User identifier.</param>
        string[] GetUserRoles(int userId);
        /// <summary>
        /// Verifies the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="account">Account.</param>
        /// <param name="password">Password.</param>
        UserInfo VerifyUser(string account, string password);
        /// <summary>
        /// Verifies the user client.
        /// </summary>
        /// <returns><c>true</c>, if user client was verifyed, <c>false</c> otherwise.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="client">Client.</param>
        bool VerifyUserClient(int userId, string client);
        /// <summary>
        /// Gets the role permissions.
        /// </summary>
        /// <returns>The role permissions.</returns>
        RolePermission[] GetRolePermissions();
    }
}
