using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Extension methods for deserializing JSON objects
    /// returned in an HttpResponseMessage
    /// </summary>
    public static partial class HttpClientExtensions {

        public static async Task<HttpResponseMessage> ForwardRequestAsync (this HttpClient client, HttpRequest req, string targetUrl) {
            var msg = new HttpRequestMessage();
            msg.CopyMethod(req)
                .CopyHeaders(req)
                .CopyQueryString(req, targetUrl);
            return await client.SendAsync(msg);
        }


        public static async Task<HttpResponseMessage> ForwardRequestAsync<T>(this HttpClient client, HttpRequest req, T content, string targetUrl) {
            var msg = new HttpRequestMessage();
            msg.CopyMethod(req)
                .CopyHeaders(req)
                .CopyContent(req,content)
                .CopyQueryString(req, targetUrl);
            return await client.SendAsync(msg);
        }

        public static HttpRequestMessage CopyMethod(this HttpRequestMessage msg, HttpRequest req) {
            if (req.Method.ToUpper() == "POST")
                msg.Method = HttpMethod.Post;
            else if (req.Method.ToUpper() == "PUT")
                msg.Method = HttpMethod.Put;
            else if (req.Method.ToUpper() == "DELETE")
                msg.Method = HttpMethod.Delete;
            else if (req.Method.ToUpper() == "GET")
                msg.Method = HttpMethod.Get;
            else if (req.Method.ToUpper() == "HEAD")
                msg.Method = HttpMethod.Head;

            return msg;
        }

        public static HttpRequestMessage CopyHeaders(this HttpRequestMessage msg, HttpRequest req) {
            foreach (var header in req.Headers.Where(h => !h.Key.StartsWith("Content-")))
                msg.Headers.Add(header.Key, header.Value.FirstOrDefault());

            return msg;
        }

        public static HttpRequestMessage CopyQueryString(this HttpRequestMessage msg, HttpRequest req, string targetUrl) {
            //copy the query string to the path
            var path = targetUrl;
            if (req.QueryString != null) {
                path += req.QueryString;
            }
            msg.RequestUri = new Uri(path, UriKind.RelativeOrAbsolute);

            return msg;
        }

        public static HttpRequestMessage CopyContent<T>(this HttpRequestMessage msg, HttpRequest req, T obj) {

            var contentType = req.ContentType ?? "application/json";
            var contentEncoding = (req.Headers.Where(h => h.Key == "Content-Encoding").Count() > 0)
                                    ? req.Headers["Content-Encoding"][0] : "UTF-8";

            var json = JToken.FromObject(obj).ToString();
            msg.Content = new StringContent(json,
                    Encoding.UTF8, "application/json");

            return msg;
        }



        /// <summary>
        /// Forwards an HttpRequestMessage from an HttpContext 
        /// through an HttpClient to a target URL
        /// </summary>
        /// <param name="client">The HttpClient object</param>
        /// <param name="context">The HttpContext, which holds the parent request</param>
        /// <param name="url">The target URL</param>
        /// <returns>An HttpResponseMessage returned by the HttpClient</returns>
        [UnTested]
        public static ActionResult ForwardForActionResult<TEntity>(this HttpClient client, HttpContext context, string url) {

#warning Untested code in this method.
            //initialize the HttpRequestMessage
            var msg = new HttpRequestMessage();

            //get the HttpRequest from the HttpContext
            var req = context.Request;

            //copy all headers from the HttpContext's request to the message
            foreach (var header in req.Headers) {
                msg.Headers.Add(header.Key, header.Value.FirstOrDefault());
            }

            // if a body exists, copy it to the message
            if (req.Body != null &&
                (req.Method.ToUpper() == "POST"
                || req.Method.ToUpper() == "PUT"))
                msg.Content = new StreamContent(req.Body);
            //needed??? msg.Content.Headers.ContentType = new MediaTypeHeaderValue(req.ContentType);
            //using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8)) {
            //    var body = reader.ReadToEnd();
            //    if (body.Length > 0)
            //        msg.Content = new StringContent(body,
            //            Encoding.UTF8, "application/json");
            //}

            //set the message's HttpMethod to the same as the parent request
            if (req.Method.ToUpper() == "POST")
                msg.Method = HttpMethod.Post;
            else if (req.Method.ToUpper() == "PUT")
                msg.Method = HttpMethod.Put;
            else if (req.Method.ToUpper() == "DELETE")
                msg.Method = HttpMethod.Delete;
            else if (req.Method.ToUpper() == "GET")
                msg.Method = HttpMethod.Get;
            else if (req.Method.ToUpper() == "HEAD")
                msg.Method = HttpMethod.Head;

            //copy the query string to the path
            var path = url;
            if (req.QueryString != null) {
                path += req.QueryString;
            }
            msg.RequestUri = new Uri(path, UriKind.RelativeOrAbsolute);

            //send the message and record the response
            var response = client.SendAsync(msg).Result;

            //get the status code of the response
            var statusCode = (int)response.StatusCode;

            //return the appropriate ActionResult ...

            //StatusCodeResult ...
            if (statusCode != 200)
                return new StatusCodeResult(statusCode);

            //ObjectResult ...
            if (msg.Method == HttpMethod.Post
                || msg.Method == HttpMethod.Put
                || msg.Method == HttpMethod.Get) {
                var obj = response.GetObject<TEntity>();
                var result = new ObjectResult(obj);
                return result;
            } else {
                var result = new StatusCodeResult(statusCode);
                return result;
            }

        }



        /// <summary>
        /// Asynchronously forwards an HttpRequestMessage from an 
        /// HttpContext through an HttpClient to a target URL
        /// </summary>
        /// <param name="client">The HttpClient object</param>
        /// <param name="context">The HttpContext, which holds the parent request</param>
        /// <param name="url">The target URL</param>
        /// <returns>An HttpResponseMessage returned by the HttpClient</returns>
        [UnTested]
        public static async Task<HttpResponseMessage> ForwardForResponseMessageResultAsync(this HttpClient client, HttpContext context, string url) {

#warning Untested code in this method.

            //initialize the HttpRequestMessage
            var msg = new HttpRequestMessage();

            //get the HttpRequest from the HttpContext
            var req = context.Request;

            //copy all headers from the HttpContext's request to the message
            foreach (var header in req.Headers) {
                Debug.WriteLine(header.Key);
                if (header.Key != "Content-Type" &&
                    header.Key != "Content-Encoding" &&
                    header.Key != "Content-Length")
                    msg.Headers.Add(header.Key, header.Value.FirstOrDefault());
            }

            // if a body exists, copy it to the message
            if (req.Body != null &&
                (req.Method.ToUpper() == "POST"
                ||
                req.Method.ToUpper() == "PUT")
                ) {
                using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8)) {
                    var body = reader.ReadToEnd();
                    if (body.Length > 0) {
                        msg.Content = new StringContent(body,
                            Encoding.UTF8, req.ContentType);
                    }
                }
            }

            //set the message's HttpMethod to the same as the parent request
            if (req.Method.ToUpper() == "POST")
                msg.Method = HttpMethod.Post;
            else if (req.Method.ToUpper() == "PUT")
                msg.Method = HttpMethod.Put;
            else if (req.Method.ToUpper() == "DELETE")
                msg.Method = HttpMethod.Delete;
            else if (req.Method.ToUpper() == "GET")
                msg.Method = HttpMethod.Get;
            else if (req.Method.ToUpper() == "HEAD")
                msg.Method = HttpMethod.Head;

            //copy the query string to the path
            var path = url;
            if (req.QueryString != null) {
                path += req.QueryString;
            }
            msg.RequestUri = new Uri(path, UriKind.RelativeOrAbsolute);

            //send the message and record the response
            var response = await client.SendAsync(msg);

            //return the HttpResponseMessage
            return response;
        }

    }

    [Obsolete("\n\n***WARNING: NEW, UNTESTED CODE.***")]
    public class UnTested : System.Attribute { }



}

