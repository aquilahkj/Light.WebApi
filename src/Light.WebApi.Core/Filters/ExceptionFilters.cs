using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Light.WebApi.Core
{
    class ExceptionFilter : IExceptionFilter
    {
        readonly IExceptionManagement exceptionManagement;
        readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(IExceptionManagement exceptionManagement, ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
            this.exceptionManagement = exceptionManagement;
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            ErrorResult result = null;
            bool logFullException = false;
            bool logPostData = false;

            if (exception is AggregateException) {
                exception = exception.InnerException;
            }
            Type type = exception.GetType();
            if (exceptionManagement.TryGetExceptionTypeFunc(type, out ExceptonTypeModel typeModel)) {
                result = typeModel.ExceptionFunc.Invoke(context, exception);
                logFullException = typeModel.LogFullException;
                logPostData = typeModel.LogPostData;
            }
            else if (exceptionManagement.TryGetExceptionCode(type, out ExceptonCodeModel codeModel)) {
                result = new ErrorResult(codeModel.Code, exception.Message);
                logFullException = codeModel.LogFullException;
                logPostData = codeModel.LogPostData;
            }
            else {
                if (exception is AuthorizeException authorizeException) {
                    logFullException = false;
                    logPostData = false;
                    result = new ErrorResult(401, authorizeException.Message);
                }
                else if (exception is PermissionException permissionException) {
                    logFullException = false;
                    logPostData = false;
                    result = new ErrorResult(403, permissionException.Message);
                }
                else if (exception is ParameterException parameterException) {
                    logFullException = false;
                    logPostData = false;
                    result = new ErrorResult(402, parameterException.Message);
                }
                else {
                    logPostData = true;
                    logFullException = true;
                    result = new ErrorResult(500, exception.Message);
                }
            }
            context.HttpContext.Response.StatusCode = 400;
            if (exceptionManagement.EnableShowDetail) {
                ErrorData[] list = {
                    new ErrorData() {
                        Name = "message",
                        Info = exception.Message
                    },
                    //new ErrorData() {
                    //    Name = "trace",
                    //    Info = exception.StackTrace
                    //},
                    //new ErrorData() {
                    //    Name = "source",
                    //    Info = exception.Source
                    //},
                    //new ErrorData() {
                    //    Name = "hrseult",
                    //    Info = exception.HResult.ToString()
                    //},
                    new ErrorData() {
                        Name = "inner",
                        Info = exception.InnerException?.Message
                    }
                };
                result.Errors = list;
            }

            context.Result = new JsonResult(result);
            if (logger != null && exceptionManagement.EnableLogger) {
                var httpContext = context.HttpContext;
                var user = httpContext.GetAccount();
                var action = httpContext.Request.Method + " " + context.HttpContext.Request.Path.Value;
                if (context.HttpContext.Request.QueryString.HasValue) {
                    action += " " + context.HttpContext.Request.QueryString.Value;
                }
                var tokens = httpContext.Request.Headers["X-Token"];
                var token = tokens.Count > 0 ? tokens[0] : null;
                StringBuilder sb = new StringBuilder();
                sb.Append(exception.Message);
                sb.Append($",action:{action}");
                if (token != null)
                    sb.Append($",token:{token}");
                if (user != null)
                    sb.Append($",user:{user}");
                if (result is ErrorResult errorResult) {
                    sb.Append($",errcode:{errorResult.Code},errmsg:{errorResult.Message}");
                }
                if (logPostData && httpContext.Request.ContentLength.HasValue && httpContext.Request.ContentLength.Value > 0) {
                    httpContext.Request.EnableRewind();
                    httpContext.Request.Body.Seek(0, 0);
                    string content;
                    using (StreamReader sr = new StreamReader(httpContext.Request.Body, Encoding.UTF8)) {
                        content = sr.ReadToEnd();
                    }
                    if (!string.IsNullOrEmpty(content)) {
                        sb.AppendLine();
                        sb.AppendLine("**Request Body Start*********");
                        sb.AppendLine(content);
                        sb.AppendLine("**Request Body End*********");
                    }
                }
                if (!logFullException) {
                    logger.LogError(sb.ToString());
                }
                else {
                    logger.LogError(exception, sb.ToString());
                }
            }
        }
    }
}
