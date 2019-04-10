using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Light.WebApi.Core
{
    public static class AuthorizeExtensions
    {
        const string UID = "UID";

        const string ACCOUNT = "ACCOUNT";

        const string USERNAME = "USERNAME";

        const string CLIENTID = "CLIENTID";

        const string ROLES = "ROLES";

        //public static void SetUserInfo(this HttpContext context, string userId, string account, string userName, string clientId)
        //{
        //    var claims = new Claim[] {
        //        new Claim(UID, userId),
        //        new Claim(ACCOUNT, account),
        //        new Claim(USERNAME, userName),
        //        new Claim(CLIENTID, clientId)
        //    };
        //    context.User.AddIdentity(new ClaimsIdentity(claims, "customize", "user", "user"));
        //}

        public static void SetUserInfo(this HttpContext context, AccountAuthorizeInfo account)
        {
            var claims = new Claim[] {
                new Claim(UID, account.LoginId),
                new Claim(ACCOUNT, account.Account),
                new Claim(USERNAME, account.UserName),
                new Claim(CLIENTID, account.Client),
                new Claim(ROLES, string.Join(',',account.Roles))
            };
            context.User.AddIdentity(new ClaimsIdentity(claims, "customize", "user", "user"));
        }


        public static void SetClientInfo(this HttpContext context, string clientId)
        {
            var claims = new Claim[] {
                new Claim(UID, clientId),
                new Claim(ACCOUNT, clientId),
                new Claim(USERNAME, clientId),
                new Claim(CLIENTID, clientId)
            };
            context.User.AddIdentity(new ClaimsIdentity(claims, "customize", "client", "client"));
        }

        public static string GetUserId(this HttpContext context)
        {
            if (context.User == null) {
                return null;
            }
            var claim = context.User.FindFirst(x => x.Type == UID);
            return claim?.Value;
        }

        public static string GetAccount(this HttpContext context)
        {
            if (context.User == null) {
                return null;
            }
            var claim = context.User.FindFirst(x => x.Type == ACCOUNT);
            return claim?.Value;
        }

        public static string GetUserName(this HttpContext context)
        {
            if (context.User == null) {
                return null;
            }
            var claim = context.User.FindFirst(x => x.Type == USERNAME);
            return claim?.Value;
        }

        public static string GetClientId(this HttpContext context)
        {
            if (context.User == null) {
                return null;
            }
            var claim = context.User.FindFirst(x => x.Type == CLIENTID);
            return claim?.Value;
        }

        public static string[] GetRoles(this HttpContext context)
        {
            if (context.User == null) {
                return null;
            }
            var claim = context.User.FindFirst(x => x.Type == ROLES);
            return claim?.Value.Split(',');
        }
    }
}
