using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi
{
    public interface IExceptionManagement
    {
        bool TryGetExceptionCode(Type type, out int errCode);
        bool TryGetExceptionTypeFunc(Type type, out Func<ExceptionContext, Exception, ResultModel> func);
    }
}