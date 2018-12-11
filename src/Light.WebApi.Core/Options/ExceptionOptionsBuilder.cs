using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    public class ExceptionOptionsBuilder
    {
        public ExceptionOptionsBuilder()
        {
        }

        readonly List<Tuple<Type, Func<ExceptionContext, Exception, ResultModel>>> typeList = new List<Tuple<Type, Func<ExceptionContext, Exception, ResultModel>>>();

        readonly List<Tuple<Type, int>> codeList = new List<Tuple<Type, int>>();

        bool exceptionLog;

        public void RegisterType<T>(Func<ExceptionContext, T, ResultModel> func) where T : Exception
        {
            var nfunc = new Func<ExceptionContext, Exception, ResultModel>((arg1, arg2) => {
                return func.Invoke(arg1, arg2 as T);
            });
            var t = new Tuple<Type, Func<ExceptionContext, Exception, ResultModel>>(typeof(T), nfunc);
            typeList.Add(t);
        }

        public void RegisterCode<T>(int errCode) where T : Exception
        {
            var t = new Tuple<Type, int>(typeof(T), errCode);
            codeList.Add(t);
        }

        public void EnableExceptionLog()
        {
            exceptionLog = true;
        }

        internal ExceptionOptions Build()
        {
            var typedict = new Dictionary<Type, Func<ExceptionContext, Exception, ResultModel>>();
            foreach (var item in typeList) {
                typedict[item.Item1] = item.Item2;
            }
            var codedict = new Dictionary<Type, int>();
            foreach (var item in codeList) {
                if (typedict.ContainsKey(item.Item1)) {
                    continue;
                }
                codedict[item.Item1] = item.Item2;
            }
            var options = new ExceptionOptions() {
                ExceptionTypes = typedict,
                ExceptionCodes = codedict,
                ExceptionLog = exceptionLog
            };
            return options;
        }
    }
}
