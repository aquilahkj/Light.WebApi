using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public class ExceptonCodeModel
    {
        public int Code;

        public bool LogFullException;

        public bool LogPostData { get; internal set; }
    }
}
