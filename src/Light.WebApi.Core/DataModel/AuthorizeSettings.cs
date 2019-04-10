using System;
namespace Light.WebApi.Core
{
    /// <summary>
    /// Authorize settings.
    /// </summary>
    public class AuthorizeSettings
    {
        /// <summary>
        /// Gets or sets the type of the cache.
        /// </summary>
        /// <value>The type of the cache.</value>
        public string CacheType { get; set; }

        /// <summary>
        /// Gets or sets the redis config.
        /// </summary>
        /// <value>The redis config.</value>
        public string RedisConfig { get; set; }

        /// <summary>
        /// Gets or sets the token key.
        /// </summary>
        /// <value>The token key.</value>
        public string TokenKey { get; set; }

        /// <summary>
        /// Gets or sets the cache expiry.
        /// </summary>
        /// <value>The cache expiry.</value>
        public int? CacheExpiry { get; set; }
    }
}
