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

        public static void SetUserInfo(this HttpContext context, string userName, string userId, string clientId)
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
            var claim = context.User.FindFirst(x => x.Type == UID);
            return claim?.Value;
        }

        public static string GetUserName(this HttpContext context)
        {
            var claim = context.User.FindFirst(x => x.Type == USERNAME);
            return claim?.Value;
        }

        public static string GetClientId(this HttpContext context)
        {
            var claim = context.User.FindFirst(x => x.Type == CLIENTID);
            return claim?.Value;
        }

        //public static void SetUserName(this HttpContext context, string userName)
        //{
        //    context.Items[USERNAME] = userName;
        //}


        //public static void SetUserId(this HttpContext context, string userId)
        //{
        //    context.Items[UID] = userId;
        //}

        //public static void SetClientId(this HttpContext context, string clientId)
        //{
        //    context.Items[CLIENTID] = clientId;
        //}

        //public static string GetUserName(this HttpContext context)
        //{
        //    return context.Items.TryGetValue(USERNAME, out object value) ? value as string : null;
        //}

        //public static string GetUserId(this HttpContext context)
        //{
        //    return context.Items.TryGetValue(UID, out object value) ? value as string : null;
        //}

        //public static string GetClientId(this HttpContext context)
        //{
        //    return context.Items.TryGetValue(CLIENTID, out object value) ? value as string : null;
        //}

    }
}
