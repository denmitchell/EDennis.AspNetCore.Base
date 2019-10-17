using Castle.DynamicProxy;
using EDennis.AspNetCore.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Logging {

    /// <summary>
    /// Castle Core Dynamic Proxy Interceptor that is used to provide
    /// Trace Logging upon entry, exit, and exceptions in all VIRTUAL
    /// methods of a class for which a Dynamic Proxy is generated.
    /// In the EDennis.AspNetCore.Base library, dynamic proxies are
    /// created during DI setup using IServiceCollection extension
    /// methods.
    /// <see cref="Web.IServiceCollectionExtensions_TraceInterceptor"/>
    /// </summary>
    public class TraceInterceptor : IInterceptor {
        private readonly ILogger logger;
        private readonly string User;

        public TraceInterceptor(ILogger logger, ScopeProperties scopeProperties = null) {
            this.logger = logger;
            User = scopeProperties?.User ?? "Anonymous";
        }

        /// <summary>
        /// If the logger is enabled at the Trace/Verbose
        /// level, use it to log the entry and exit to a
        /// given method, as well as any thrown exceptions
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation) {
            if (!logger.IsEnabled(LogLevel.Trace)) {
                try {
                    invocation.Proceed();
                } catch (Exception ex) {
                    var args = invocation.Arguments.FormatCompact();
                    var method = invocation.Method.GetFriendlyName();
                    var scope = GetScope(invocation);
                    using (logger.BeginScope(scope)) {
                        if (ex is TargetInvocationException && ex.InnerException != null)
                            logger.LogError(ex.InnerException, "For {User}, Exception {Method}(" + args + ") { Message}", User, method, ex.InnerException.Message);
                    }
                    throw ex.InnerException;
                }
            } else {
                var scope = GetScope(invocation);
                var args = invocation.Arguments.FormatCompact();
                var method = invocation.Method.GetFriendlyName();
                using (logger.BeginScope(scope)) {
                    logger.LogTrace("For {User}, Enter {Method}(" + args + ")", User, method);
                    try {
                        invocation.Proceed();
                    } catch (Exception ex) {
                        if (ex is TargetInvocationException && ex.InnerException != null)
                            logger.LogError(ex.InnerException, "For {User}, Exception {Method}(" + args + ") { Message}", User, method, ex.InnerException.Message);
                        throw ex.InnerException;
                    }
                    if (invocation.ReturnValue != null) {
                        object returnValue = null;
                        if (invocation.ReturnValue.GetType().IsGenericType && invocation.ReturnValue.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                            returnValue = ((dynamic)invocation.ReturnValue).Result;
                        else returnValue = invocation.ReturnValue;
                        var additionalScope = GetScope("ReturnValue", returnValue.Format());
                        using (logger.BeginScope(additionalScope)) {
                            logger.LogTrace("For {User}, Exit {Method}(" + args + ")", User, method);
                        }
                    } else {
                        logger.LogTrace("For {User}, Exit {Method}(" + args + ")", User, method);
                    }
                }
            }
        }


        private IEnumerable<KeyValuePair<string, object>> GetScope(IInvocation invocation) {
            List<KeyValuePair<string, object>> logScope = new List<KeyValuePair<string, object>>();
            var args = invocation.Arguments.Format();
            var parms = invocation.Method.GetParameters();
            for (int i = 0; i < parms.Length; i++) {
                logScope.Add(KeyValuePair.Create(parms[i].Name, args[i] ?? parms[i].DefaultValue));
            }
            return logScope;
        }

        private IEnumerable<KeyValuePair<string, object>> GetScope(string key, object value) {
            var scope = new List<KeyValuePair<string, object>> {
                KeyValuePair.Create(key, value)
            };
            return scope;
        }

    }

    public static class Extensions {

        public static object[] Format(this object[] objs) {
            var retVal = new object[objs.Length];
            for (int i = 0; i < objs.Length; i++)
                retVal[i] = objs[i].Format();
            return retVal;
        }

        public static object Format(this object obj) {
            object retVal;
            if (obj.GetType().IsPrimitive)
                retVal = obj;
            else {
                var str = JsonSerializer.Serialize(obj);
                retVal = str;
            }
            return retVal;
        }



        public static string FormatCompact(this object[] objs, bool compact = true, bool removeQuotes = true, bool escapeBraces = true) {
            var sb = new StringBuilder();

            for (int i = 0; i < objs.Length; i++) {
                if (i > 0)
                    sb.Append(",");
                sb.Append(objs[i].FormatCompact(compact, removeQuotes, escapeBraces));
            }
            return sb.ToString();
        }

        public static object FormatCompact(this object obj, bool compact = true, bool removeQuotes = true, bool escapeBraces = true) {
            object retVal;
            if (obj.GetType().IsPrimitive)
                retVal = obj;
            else {
                var str = JsonSerializer.Serialize(obj);
                if (compact)
                    str = Regex
                        .Replace(str, "\"[A-Za-z0-9_]+\":", "");
                if (escapeBraces)
                    str = str.Replace("{", "{{")
                        .Replace("}", "}}");
                if (removeQuotes)
                    str = str.Replace("\"", "");
                if (str.Length > 80)
                    str = str.Substring(0, 80) + "...";
                retVal = str;
            }
            return retVal;
        }

        public static string GetFriendlyName(this MethodInfo method) {
            return $"{GetFriendlyName(method.DeclaringType)}.{method.Name}";
        }

        /// <summary>
        /// from https://stackoverflow.com/a/26429045/10896865
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFriendlyName(this Type type) {
            string friendlyName = type.Name;
            if (type.IsGenericType) {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0) {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i) {
                    string typeParamName = GetFriendlyName(typeParameters[i]);
                    friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
                }
                friendlyName += ">";
            }

            return friendlyName;
        }
    }
}