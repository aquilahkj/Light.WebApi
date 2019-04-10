using System;
namespace Light.WebApi.Core
{
    /// <summary>
    /// Account authorize info.
    /// </summary>
    public class AccountAuthorizeInfo
    {
        /// <summary>
        /// Gets or sets the login identifier.
        /// </summary>
        /// <value>The login identifier.</value>
        public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public string Client { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public string[] Roles { get; set; }
    }
}
