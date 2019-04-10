using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public interface IExceptionManagement
    {
        bool TryGetExceptionCode(Type type, out ExceptonCodeModel model);
        bool TryGetExceptionTypeFunc(Type type, out ExceptonTypeModel model);
        bool EnableLogger { get; }
        bool EnableShowDetail { get; }
    }
}