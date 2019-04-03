using System;
namespace Light.WebApi.Core
{
    public class AccountAuthorizeInfo
    {
        public string LoginId { get; set; }

        public string Account { get; set; }

        public string UserName { get; set; }

        public string Guid { get; set; }

        public string Client { get; set; }

        public DateTime CreateTime { get; set; }

        public string[] Roles { get; set; }
    }
}
