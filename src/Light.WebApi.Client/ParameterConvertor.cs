using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web;

namespace Light.WebApi.Client
{
    public static class ParameterConvertor
    {
        delegate object GetValueHandler(object source);

        class GetPropertyHandler
        {
            private GetValueHandler mGetValue;
            private PropertyInfo mProperty;
            private string mName;

            public GetValueHandler Get {
                get {
                    return this.mGetValue;
                }
            }

            public PropertyInfo Property {
                get {
                    return this.mProperty;
                }
            }

            public string Name {
                get {
                    return this.mName;
                }
            }

            public GetPropertyHandler(PropertyInfo property)
            {
                if (property.CanRead) {
                    this.mGetValue = PropertyGetHandler(property);
                }
                this.mProperty = property;
                this.mName = property.Name;
            }

            private static readonly Dictionary<PropertyInfo, GetValueHandler> mPropertyGetHandlers = new Dictionary<PropertyInfo, GetValueHandler>();

            public static GetValueHandler PropertyGetHandler(PropertyInfo property)
            {
                GetValueHandler handler;
                if (mPropertyGetHandlers.ContainsKey(property)) {
                    return mPropertyGetHandlers[property];
                }
                lock (mPropertyGetHandlers) {
                    if (mPropertyGetHandlers.ContainsKey(property)) {
                        return mPropertyGetHandlers[property];
                    }
                    handler = CreatePropertyGetHandler(property);
                    mPropertyGetHandlers.Add(property, handler);
                }
                return handler;
            }

            private static GetValueHandler CreatePropertyGetHandler(PropertyInfo property)
            {
                DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, property.DeclaringType.GetTypeInfo().Module);
                ILGenerator iLGenerator = method.GetILGenerator();
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.EmitCall(OpCodes.Callvirt, property.GetMethod, null);
                EmitBoxIfNeeded(iLGenerator, property.PropertyType);
                iLGenerator.Emit(OpCodes.Ret);
                return (GetValueHandler)method.CreateDelegate(typeof(GetValueHandler));
            }

            private static void EmitBoxIfNeeded(ILGenerator il, Type type)
            {
                if (type.GetTypeInfo().IsValueType) {
                    il.Emit(OpCodes.Box, type);
                }
            }
        }

        static Dictionary<Type, List<GetPropertyHandler>> TypeDict = new Dictionary<Type, List<GetPropertyHandler>>();

        public static string Convert(object obj)
        {
            if (object.Equals(obj, null)) {
                throw new ArgumentNullException(nameof(obj));
            }
            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();
            if (!TypeDict.TryGetValue(type, out List<GetPropertyHandler> list)) {
                lock (TypeDict) {
                    if (!TypeDict.TryGetValue(type, out list)) {
                        var properties = typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        list = new List<GetPropertyHandler>();
                        foreach (PropertyInfo propertie in properties) {
                            var item = new GetPropertyHandler(propertie);
                            list.Add(item);
                        }
                        TypeDict.Add(type, list);
                    }
                }
            }
            var sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++) {
                var handler = list[i];
                var name = handler.Name;
                var value = handler.Get(obj);
                if (!object.Equals(value, null)) {
                    var str = value.ToString();
                    value = HttpUtility.UrlEncode(str);
                }
                sb.Append(string.Concat(name, "=", value));
                if (i < list.Count - 1) {
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }

    }
}
