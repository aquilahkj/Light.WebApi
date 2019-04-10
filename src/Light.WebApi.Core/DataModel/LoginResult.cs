using System;
namespace Light.WebApi.Core
{
    /// <summary>
    /// Login result.
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public int Result { get; set; }

        /// <summary>
        /// Login message, if login failed, show the reason
        /// </summary>
        /// <value>The reason.</value>
        public string Message { get; set; }

        /// <summary>
        /// Get the latest login time.
        /// </summary>
        /// <value>The latest login time.</value>
        public DateTime? LatestLoginTime { get; set; }

        ///// <summary>
        ///// Gets or sets the error times.
        ///// </summary>
        ///// <value>The error times.</value>
        //public int ErrorTimes { get; set; }

        /// <summary>
        /// Login Token
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }
    }
}
