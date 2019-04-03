namespace Light.WebApi.Core
{
    public interface IAuthorizeManagement
    {
        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="password">Password.</param>
        string EncryptPassword(string password);
        /// <summary>
        /// Creates the user token.
        /// </summary>
        /// <returns>The user token.</returns>
        /// <param name="info">Info.</param>
        //string CreateUserToken(AccountAuthorizeInfo info);
        /// <summary>
        /// Gets the authorize.
        /// </summary>
        /// <returns>The authorize.</returns>
        /// <param name="info">Info.</param>
        AccountAuthorizeInfo GetAuthorize(TokenInfo info);
        /// <summary>
        /// Gets the system client identifier.
        /// </summary>
        /// <returns>The system client identifier.</returns>
        /// <param name="token">Token.</param>
        string GetSystemClientId(string token);
        /// <summary>
        /// Parses the user token.
        /// </summary>
        /// <returns>The user token.</returns>
        /// <param name="token">Token.</param>
        TokenInfo ParseUserToken(string token);
        /// <summary>
        /// Removes the authorize.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="userId">User identifier.</param>
        void RemoveAuthorize(string client, string userId);
        /// <summary>
        /// Sets the authorize.
        /// </summary>
        /// <param name="info">Info.</param>
        void SetAuthorize(AccountAuthorizeInfo info);
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Light.WebApi.Core.IAuthorizeManagement"/> test mode.
        /// </summary>
        /// <value><c>true</c> if test mode; otherwise, <c>false</c>.</value>
        bool TestMode { get; }
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
        //string[] GetUserRoles(int userId);
        /// <summary>
        /// Verifies the login user.
        /// </summary>
        /// <returns>The login user.</returns>
        /// <param name="account">Account.</param>
        /// <param name="password">Password.</param>
        /// <param name="client">Client.</param>
        string VerifyLoginUser(string account, string password, string client);
        /// <summary>
        /// Valids the role authorize.
        /// </summary>
        /// <returns><c>true</c>, if role authorize was valided, <c>false</c> otherwise.</returns>
        /// <param name="role">Role.</param>
        /// <param name="action">Action.</param>
        bool ValidRoleAuthorize(string role, string action);
        /// <summary>
        /// Gets the user permission.
        /// </summary>
        /// <returns>The user permission.</returns>
        /// <param name="roles">Roles.</param>
        string[] GetUserPermission(string[] roles);
        /// <summary>
        /// Resets the permission.
        /// </summary>
        void ResetPermission();
    }
}