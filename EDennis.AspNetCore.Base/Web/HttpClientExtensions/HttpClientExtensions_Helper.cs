using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

using static EDennis.AspNetCore.Base.Web.TestingActionFilter;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Web {


    #region Helper Methods

    public static class HttpRequestMessageExtensions {

        private static void UseTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            if (testDbType == DbType.InMemory) {
                var key = HDR_USE_INMEMORY;
                msg.Headers.Add(key, testDb);
            }
        }

        private static void DropTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            if (testDbType == DbType.InMemory) {
                var key = HDR_DROP_INMEMORY;
                msg.Headers.Add(key, testDb);
            }
        }

    }


    public static class HttpResponseMessageExtensions {
        public static T GetObject<T>(this HttpResponseMessage message) {
            var json = message.Content.ReadAsStringAsync().Result;
            return JToken.Parse(json).ToObject<T>();
        }
        public static async Task<T> GetObjectAsync<T>(this HttpResponseMessage message) {
            var json = await message.Content.ReadAsStringAsync();
            return JToken.Parse(json).ToObject<T>();
        }
    }


    public static class IApiProxyExtensions {

        /// <summary>
        /// Gets the url of a controller defined in an "Apis" section
        /// of the configuration
        /// </summary>
        /// <param name="proxy">the API proxy object</param>
        /// <param name="config">the configuration object</param>
        /// <param name="controllerName">the name of the target controller</param>
        /// <returns>the URL defined in config for the target controller</returns>
        public static string GetControllerUrl(this IApiProxy proxy, IConfiguration config, string controllerName) {
            var apiName = proxy.GetType().Name;
            var url = config[$"Apis:{apiName}:ControllerUrls:{controllerName}"];
            return url;
        }

    }


    public static class UriExtensions {

        public static Uri At(this Uri uri, params object[] id) {

            var uriParts = uri.PathAndQuery.Split('?');

            var sb = new StringBuilder();
            foreach (var val in id) {
                sb.Append("/");
                sb.Append(ToFormattedString(val));
            }

            var newUri = TrimTrailingSlash(uriParts[0]) + sb.ToString();
            if (uriParts.Length > 1)
                newUri += "?" + uriParts[1];

            UriKind uriKind = UriKind.Relative;
            if (newUri.StartsWith("http"))
                uriKind = UriKind.Absolute;

            return new Uri(newUri, uriKind);
        }

        public static Uri Where(this Uri uri, string key, object value) {
            var uriParts = uri.PathAndQuery.Split('?');
            var separator = "?";
            if (uriParts.Length > 1)
                separator = "&";

            UriKind uriKind = UriKind.Relative;
            if (uriParts[0].StartsWith("http"))
                uriKind = UriKind.Absolute;

            return new Uri(uriParts[0] + $"{separator}{key}={ToFormattedString(value)}", uriKind);
        }

        public static string ToFormattedString(object value) {
            if (value is DateTime val) {
                if (val.Hour > 0 || val.Minute > 0 || val.Second > 0) {
                    return val.ToString("yyyyMMddTHHmmss");
                } else {
                    return val.ToString("yyyyMMdd");
                }
            } else if (value is bool) {
                return (bool)value ? "true" : "false";
            } else
                return value.ToString();
        }

        private static string TrimTrailingSlash(string str) {
            if (str.EndsWith("/") || str.EndsWith("\\"))
                str = str.Substring(0, str.Length - 1);
            return str;
        }

    }

    #endregion

}

