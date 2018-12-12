using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Light.WebApi.Core
{
    public static class AuthorizeExtensions
    {
        const string UID = "UID";

        const string USERNAME = "USERNAME";

        const string CLIENTID = "CLIENTID";

        public static void SetUserInfo(this HttpContext context, string userId, string userName, string clientId)
        {
            var claims = new Claim[] {
                new Claim(UID, userId),
                new Claim(USERNAME, userName),
                new Claim(CLIENTID, clientId)
            };
            context.User.AddIdentity(new ClaimsIdentity(claims, "customize", "user", "user"));
        }

        public static void SetClientInfo(this HttpContext context, string clientId)
        {
            var claims = new Claim[] {
                new Claim(UID, clientId),
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
    }
}
