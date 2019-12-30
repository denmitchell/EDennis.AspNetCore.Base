﻿using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Logging.Extensions {
    public static class ILoggerExtensions {
        /// <summary>
        /// Returns the minimum log level for a given logger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static LogLevel EnabledAt<T>(this ILogger<T> logger) {
            for (int i = (int)LogLevel.Trace; i < (int)LogLevel.None; i++)
                if (logger.IsEnabled((LogLevel)i))
                    return (LogLevel)i;

            return LogLevel.None;
        }

        /// <summary>
        /// Returns the minimum log level for a given logger
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static LogLevel EnabledAt(this ILogger logger) {
            for (int i = (int)LogLevel.Trace; i < (int)LogLevel.None; i++)
                if (logger.IsEnabled((LogLevel)i))
                    return (LogLevel)i;

            return LogLevel.None;
        }

        public static IEnumerable<KeyValuePair<string, object>> GetScope(this ILogger logger, MethodExecutionArgs args) {
            List<KeyValuePair<string, object>> logScope = new List<KeyValuePair<string, object>>();
            var formatted = args.Arguments.Format();
            var parms = args.Method.GetParameters();
            for (int i = 0; i < parms.Length; i++) {
                logScope.Add(KeyValuePair.Create(parms[i].Name, formatted[i] ?? parms[i].DefaultValue));
            }
            if (args.ReturnValue != null) {
                var returnValue = JsonSerializer.Serialize(args.ReturnValue);
                logScope.Add(KeyValuePair.Create("ReturnValue", (object)returnValue));
            }
            return logScope;
        }

        public static IEnumerable<KeyValuePair<string, object>> GetScope(this ILogger logger, string key, object value) {
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
