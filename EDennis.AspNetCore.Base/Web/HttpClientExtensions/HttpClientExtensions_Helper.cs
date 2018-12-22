using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {


    #region Helper Methods

    /// <summary>
    /// Extension methods for deserializing JSON objects
    /// returned in an HttpResponseMessage
    /// </summary>
    public static class HttpResponseMessageExtensions {

        /// <summary>
        /// Synchronously deserializes an object from JSON
        /// and returns it
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="message">the HttpResponseMessage object</param>
        /// <returns>the deserialized object</returns>
        public static T GetObject<T>(this HttpResponseMessage message) {
            var json = message.Content.ReadAsStringAsync().Result;
            return JToken.Parse(json).ToObject<T>();
        }

        /// <summary>
        /// Asynchronously deserializes an object from JSON
        /// and returns it
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="message">the HttpResponseMessage object</param>
        /// <returns>the deserialized object</returns>
        public static async Task<T> GetObjectAsync<T>(this HttpResponseMessage message) {
            var json = await message.Content.ReadAsStringAsync();
            return JToken.Parse(json).ToObject<T>();
        }
    }

    /// <summary>
    /// Extension methods for the IApiProxy interface
    /// </summary>
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


    /// <summary>
    /// Extension methods for Uri
    /// </summary>
    public static class UriExtensions {

        /// <summary>
        /// Builds a new Uri by appending and Id to the path
        /// </summary>
        /// <param name="uri">the base Uri</param>
        /// <param name="id">the Id to append</param>
        /// <returns>a new Uri like .../{id} or .../{id1}/{id2}</returns>
        public static Uri At(this Uri uri, params object[] id) {

            //assume that the Uri could have a query segment;
            //so, isolate that segment from the rest
            var uriParts = uri.PathAndQuery.Split('?');

            //using a string builder, build the id segment(s)
            //of the Uri path
            var sb = new StringBuilder();
            foreach (var val in id) {
                sb.Append("/");
                sb.Append(ToFormattedString(val));
            }

            //trim trailing slashes in the path
            var newUri = TrimTrailingSlash(uriParts[0]) + sb.ToString();

            //re-append the query segment, if present
            if (uriParts.Length > 1)
                newUri += "?" + uriParts[1];

            //detect and explicitly set the kind of Uri
            UriKind uriKind = UriKind.Relative;
            if (newUri.StartsWith("http"))
                uriKind = UriKind.Absolute;

            //return the new Uri with the Id segment(s) appended
            return new Uri(newUri, uriKind);
        }

        /// <summary>
        /// Provides a fluent extension method for building
        /// query strings
        /// </summary>
        /// <param name="uri">the base Uri</param>
        /// <param name="key">the query string key</param>
        /// <param name="value">the query string value</param>
        /// <returns></returns>
        public static Uri Where(this Uri uri, string key, object value) {

            //handle if the Uri already containst a query segment
            var uriParts = uri.PathAndQuery.Split('?');
            var separator = "?";
            if (uriParts.Length > 1)
                separator = "&";

            //detect and explicitly set the Uri kind
            UriKind uriKind = UriKind.Relative;
            if (uriParts[0].StartsWith("http"))
                uriKind = UriKind.Absolute;

            //add the new query string segment and return it
            return new Uri(uriParts[0] + $"{separator}{key}={ToFormattedString(value)}", uriKind);
        }

        /// <summary>
        /// Format the a string in a manner that is suitable
        /// for a query string value
        /// </summary>
        /// <param name="value">the value to format</param>
        /// <returns>the formatted value</returns>
        public static string ToFormattedString(object value) {
            //handle date format as ISO
            if (value is DateTime val) {
                if (val.Hour > 0 || val.Minute > 0 || val.Second > 0) {
                    return val.ToString("yyyyMMddTHHmmss");
                } else {
                    return val.ToString("yyyyMMdd");
                }
            //handle boolean format
            } else if (value is bool) {
                return (bool)value ? "true" : "false";
            //otherwise, accept default for ToString()
            } else
                return value.ToString();
        }

        /// <summary>
        /// Remove any trailing slashes from a string, 
        /// which is useful for Uri-building
        /// </summary>
        /// <param name="str">the string to trim</param>
        /// <returns>the trimmed string</returns>
        private static string TrimTrailingSlash(string str) {
            if (str.EndsWith("/") || str.EndsWith("\\"))
                str = str.Substring(0, str.Length - 1);
            return str;
        }

    }

    #endregion

}

