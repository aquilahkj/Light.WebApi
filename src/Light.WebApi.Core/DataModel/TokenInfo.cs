namespace Light.WebApi.Core
{
    /// <summary>
    /// Token info.
    /// </summary>
    public class TokenInfo
    {
        private readonly string userId;
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId {
            get {
                return userId;
            }
        }

        private readonly string client;
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        public string Client {
            get {
                return client;
            }
        }

        private readonly string guid;
        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public string Guid {
            get {
                return guid;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.TokenInfo"/> class.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="client">Client.</param>
        /// <param name="guid">GUID.</param>
        public TokenInfo(string userId, string client, string guid)
        {
            this.userId = userId;
            this.client = client;
            this.guid = guid;
        }
    }
}