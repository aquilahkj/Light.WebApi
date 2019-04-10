using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public class ExceptonTypeModel
    {
        public Func<ExceptionContext, Exception, ErrorResult> ExceptionFunc;

        public bool LogFullException;

        public bool LogPostData { get; internal set; }
    }
}
