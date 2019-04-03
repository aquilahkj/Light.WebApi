using System;
namespace Light.WebApi.Core
{
    public class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>The avatar.</value>
        public string Avatar { get; set; }
    }
}
