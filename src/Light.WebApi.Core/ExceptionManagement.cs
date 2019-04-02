using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    internal class ExceptionManagement : IExceptionManagement
    {
        readonly Dictionary<Type, ExceptonTypeModel> exceptionTypes;
        readonly Dictionary<Type, ExceptonCodeModel> exceptionCodes;
        readonly bool enableLogger;

        public bool EnableLogger {
            get {
                return enableLogger;
            }
        }

        public ExceptionManagement(ExceptionOptions options)
        {
            exceptionTypes = options.ExceptionTypes;
            exceptionCodes = options.ExceptionCodes;
            enableLogger = options.EnableLogger;
        }

        public bool TryGetExceptionTypeFunc(Type type, out ExceptonTypeModel model)
        {
            if (exceptionTypes == null) {
                model = null;
                return false;
            }
            return exceptionTypes.TryGetValue(type, out model);
        }

        public bool TryGetExceptionCode(Type type, out ExceptonCodeModel model)
        {
            if (exceptionCodes == null) {
                model = null;
                return false;
            }
            return exceptionCodes.TryGetValue(type, out model);
        }

    }
}
