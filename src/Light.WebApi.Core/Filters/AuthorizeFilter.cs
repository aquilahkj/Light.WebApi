using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    class AuthorizeFilter : IActionFilter
    {
        private readonly IAuthorizeManagement authorizeManagement;
        private readonly IPermissionManagement permissionManagement;

        public AuthorizeFilter(IAuthorizeManagement authorizeManagement, IPermissionManagement permissionManagement)
        {
            this.permissionManagement = permissionManagement;
            this.authorizeManagement = authorizeManagement;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor) {
                var authorizeAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizePermissionAttribute), true);
                if (authorizeAttributes.Length > 0) {
                    var httpContext = context.HttpContext;
                    var authorizeAttribute = (AuthorizePermissionAttribute)authorizeAttributes[0];
                    var request = context.HttpContext.Request;
                    var tokens = request.Headers["x-token"];
                    string token;
                    if (tokens.Count == 0) {
                        if (authorizeManagement.TestMode) {
                            httpContext.SetClientInfo("test");
                            return;
                        }
                        else {
                            throw new AuthorizeException(SR.TokenNotExists, string.Empty);
                        }
                    }
                    else {
                        token = tokens[0];
                    }
                    if (string.IsNullOrEmpty(token) || token.Length < 20) {
                        throw new AuthorizeException(SR.TokenFormatError, token);
                    }
                    if (authorizeAttribute.Type == AuthorizeType.System) {
                        var client = authorizeManagement.GetSystemClientId(token);
                        if (client == null) {
                            throw new AuthorizeException(SR.TokenError, token);
                        }
                        httpContext.SetClientInfo(client);
                    }
                    else {
                        var tokenInfo = authorizeManagement.ParseUserToken(token);
                        if (tokenInfo == null) {
                            throw new AuthorizeException(SR.UserTokenError, token);
                        }
                        var account = authorizeManagement.GetAuthorize(tokenInfo);
                        if (account == null) {
                            throw new AuthorizeException(SR.UserNotLogin, token);
                        }
                        if (account.Guid != tokenInfo.Guid) {
                            throw new AuthorizeException(SR.UserHasLogin, token);
                        }
                        httpContext.SetUserInfo(account.LoginId, account.UserName, account.Client);

                        if (authorizeAttribute.Type == AuthorizeType.UserAction) {
                            var action = context.HttpContext.Request.Path;
                            if (!ValidUserPermission(account.Roles, action)) {
                                throw new PermissionException(SR.UserNotPermission, account.UserName, action);
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
                if (permissionManagement.ValidRoleAuthorize(role, action)) {
                    return true;
                }
            }
            return false;
        }
    }
}
