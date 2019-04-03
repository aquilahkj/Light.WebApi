using System;
namespace Light.WebApi.Core
{
    public class AdminUser
    {
        readonly string account;

        public string Account
        {
            get {
                return account;
            }
        }

        readonly string userName;

        public string UserName
        {
            get {
                return userName;
            }
        }

        readonly string password;

        public string Password
        {
            get {
                return password;
            }
        }

        readonly int userId;

        public int UserId
        {
            get
            {
                return userId;
            }
        }

        public AdminUser(int userId, string account, string userName, string password)
        {

            if (string.IsNullOrEmpty(account)) {
                throw new ArgumentException("value is null", nameof(account));
            }

            if (string.IsNullOrEmpty(userName)) {
                throw new ArgumentException("value is null", nameof(userName));
            }

            if (string.IsNullOrEmpty(password)) {
                throw new ArgumentException("value is null", nameof(password));
            }
            this.userId = userId;
            this.account = account;
            this.userName = userName;
            this.password = password;
        }
    }
}
