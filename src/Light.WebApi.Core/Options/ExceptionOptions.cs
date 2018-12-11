using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public class ExceptionOptions
    {
        public ExceptionOptions()
        {
        }

        public Dictionary<Type, Func<ExceptionContext, Exception, ResultModel>> ExceptionTypes { get; internal set; }
        public Dictionary<Type, int> ExceptionCodes { get; internal set; }
        public bool ExceptionLog { get; internal set; }
    }
}
