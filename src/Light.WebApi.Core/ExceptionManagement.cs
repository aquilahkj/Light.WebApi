using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public class ExceptionManagement : IExceptionManagement
    {
        readonly Dictionary<Type, Func<ExceptionContext, Exception, ResultModel>> exceptionTypes;
        readonly Dictionary<Type, int> exceptionCodes;

        public ExceptionManagement(ExceptionOptions options)
        {
            exceptionTypes = options.ExceptionTypes;
            exceptionCodes = options.ExceptionCodes;
        }

        public bool TryGetExceptionTypeFunc(Type type, out Func<ExceptionContext, Exception, ResultModel> func)
        {
            if (exceptionTypes == null) {
                func = null;
                return false;
            }
            return exceptionTypes.TryGetValue(type, out func);
        }

        public bool TryGetExceptionCode(Type type, out int errCode)
        {
            if (exceptionCodes == null) {
                errCode = 0;
                return false;
            }
            return exceptionCodes.TryGetValue(type, out errCode);
        }

    }
}
