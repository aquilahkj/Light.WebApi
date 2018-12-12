using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public class ExceptonTypeModel
    {
        public Func<ExceptionContext, Exception, ResultModel> ExceptionFunc;

        public bool LogFullException;

        public bool LogPostData { get; internal set; }
    }
}
