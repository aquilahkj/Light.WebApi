using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    class AuthorizeFilter : IActionFilter
    {
        private readonly IAuthorizeManagement authorize;
        private readonly IPermissionManagement permission;

        public AuthorizeFilter(IAuthorizeManagement authorize, IPermissionManagement permission)
        {
            this.permission = permission;
            this.authorize = authorize;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor) {
                var authorizeAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizePermissionAttribute), true);
                if (authorizeAttributes.Length > 0) {
                    var authorizeAttribute = (AuthorizePermissionAttribute)authorizeAttributes[0];
                    var request = context.HttpContext.Request;
                    var tokens = request.Headers["x-token"];
                    string token;
                    if (tokens.Count == 0) {
                        throw new AuthorizeException(SR.TokenNotExists);
                    }
                    else {
                        token = tokens[0];
                    }
                    if (string.IsNullOrEmpty(token) || token.Length < 20) {
                        throw new AuthorizeException(SR.TokenFormatError);
                    }
                    var httpContext = context.HttpContext;

                    if (authorizeAttribute.Type == AuthorizeType.System) {
                        var client = authorize.GetSystemClientId(token);
                        if (client == null) {
                            throw new AuthorizeException(SR.TokenError);
                        }
                        httpContext.SetClientInfo(client);
                    }
                    else {
                        var tokenInfo = authorize.ParseUserToken(token);
                        if (tokenInfo == null) {
                            throw new AuthorizeException(SR.UserTokenError);
                        }
                        var account = authorize.GetAuthorize(tokenInfo);
                        if (account == null) {
                            throw new AuthorizeException(SR.UserNotLogin);
                        }
                        if (account.Guid != tokenInfo.Guid) {
                            throw new AuthorizeException(SR.UserHasLogin);
                        }
                        httpContext.SetUserInfo(account.LoginId, account.UserName, account.Client);

                        if (authorizeAttribute.Type == AuthorizeType.UserAction) {
                            var action = context.HttpContext.Request.Path;
                            if (!ValidUserPermission(account.Roles, action)) {
                                throw new PermissionException(SR.UserNotPermission);
                            }
                        }
                    }
                }
            }
        }

        bool ValidUserPermission(string[] roles, string action)
        {
            if (roles == null || roles.Length == 0) {
                return false;
            }
            foreach (var role in roles) {
                if (role == "admin") {
                    return true;
                }
                if (permission.ValidUserAuthorize(role, action)) {
                    return true;
                }
            }
            return false;
        }
    }
}
