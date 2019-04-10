using System;
using System.ComponentModel.DataAnnotations;

namespace Light.WebApi.Core
{
    public class LoginModel
    {
        /// <summary>
        /// User Account
        /// </summary>
        /// <value>The account.</value>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        /// <value>The password.</value>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Client Flag
        /// </summary>
        /// <value>The client.</value>
        //[Required]
        public string Client { get; set; }

    }
}
