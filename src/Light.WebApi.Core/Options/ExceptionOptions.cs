using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    class ExceptionOptions
    {
        public Dictionary<Type, ExceptonTypeModel> ExceptionTypes { get; internal set; }
        public Dictionary<Type, ExceptonCodeModel> ExceptionCodes { get; internal set; }
        public bool EnableLogger { get; internal set; }
        public bool EnableShowDetail { get; internal set; }
    }
}
