using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Light.WebApi.Core
{
    /// <summary>
    /// Exception options builder.
    /// </summary>
    public class ExceptionOptionsBuilder
    {
        public ExceptionOptionsBuilder()
        {
        }

        readonly List<Tuple<Type, ExceptonTypeModel>> typeList = new List<Tuple<Type, ExceptonTypeModel>>();

        readonly List<Tuple<Type, ExceptonCodeModel>> codeList = new List<Tuple<Type, ExceptonCodeModel>>();

        bool exceptionLogger;

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="func">Func.</param>
        /// <param name="logFullException">If set to <c>true</c> log full exception.</param>
        /// <param name="logPostData">If set to <c>true</c> log post data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void RegisterType<T>(Func<ExceptionContext, T, ErrorResult> func, bool logFullException = false, bool logPostData = false) where T : Exception
        {
            var nfunc = new Func<ExceptionContext, Exception, ErrorResult>((arg1, arg2) => {
                return func.Invoke(arg1, arg2 as T);
            });
            var model = new ExceptonTypeModel() {
                ExceptionFunc = nfunc,
                LogFullException = logFullException,
                LogPostData = logPostData
            };
            var t = new Tuple<Type, ExceptonTypeModel>(typeof(T), model);
            typeList.Add(t);
        }

        /// <summary>
        /// Registers the code.
        /// </summary>
        /// <param name="errCode">Error code.</param>
        /// <param name="logFullException">If set to <c>true</c> log full exception.</param>
        /// <param name="logPostData">If set to <c>true</c> log post data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void RegisterCode<T>(int errCode, bool logFullException = false, bool logPostData = false) where T : Exception
        {
            var model = new ExceptonCodeModel() {
                Code = errCode,
                LogFullException = logFullException,
                LogPostData = logPostData
            };
            var t = new Tuple<Type, ExceptonCodeModel>(typeof(T), model);
            codeList.Add(t);
        }

        public void EnableExceptionLogger()
        {
            exceptionLogger = true;
        }

        internal ExceptionOptions Build()
        {
            var typedict = new Dictionary<Type, ExceptonTypeModel>();
            foreach (var item in typeList) {
                typedict[item.Item1] = item.Item2;
            }
            var codedict = new Dictionary<Type, ExceptonCodeModel>();
            foreach (var item in codeList) {
                if (typedict.ContainsKey(item.Item1)) {
                    continue;
                }
                codedict[item.Item1] = item.Item2;
            }
            var options = new ExceptionOptions() {
                ExceptionTypes = typedict,
                ExceptionCodes = codedict,
                EnableLogger = exceptionLogger
            };
            return options;
        }
    }
}
