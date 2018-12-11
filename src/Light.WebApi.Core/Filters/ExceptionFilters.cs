using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    class ExceptionFilter : IExceptionFilter
    {
        readonly IExceptionManagement exceptionManagement;

        public ExceptionFilter(IExceptionManagement exceptionManagement)
        {
            this.exceptionManagement = exceptionManagement;
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            ResultModel result = null;
            if (exception is AggregateException) {
                exception = exception.InnerException;
            }
            Type type = exception.GetType();
            if (exceptionManagement.TryGetExceptionTypeFunc(type, out Func<ExceptionContext, Exception, ResultModel> func)) {
                result = func.Invoke(context, exception);
            }
            else if (exceptionManagement.TryGetExceptionCode(type, out int errCode)) {
                result = ResultModel.CreateErrorResult(errCode, exception.Message);
            }
            else {
                if (exception is AuthorizeException authorizeException) {
                    result = ResultModel.CreateErrorResult(-1, authorizeException.Message);
                }
                else if (exception is PermissionException permissionException) {
                    result = ResultModel.CreateErrorResult(-2, permissionException.Message);
                }
                else {
                    result = ResultModel.CreateErrorResult(-99, exception.Message);
                }
            }
            context.Result = new JsonResult(result);
        }


    }
}
