using Flurl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public static class HttpClientExtensions {

        public static HttpClientResult<T> Get<T>(this HttpClient client, string relativeUrlFromBase) {
            return client.GetAsync<T>(relativeUrlFromBase).Result;
        }

        public static async Task<HttpClientResult<T>> GetAsync<T>(
                this HttpClient client, string relativeUrlFromBase) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);
            var response = await client.GetAsync(url);

            var so = new HttpClientResult<T> {
                StatusCode = (int)response.StatusCode
            };

            if (so.StatusCode > 299)
                return so;

            if (response.Content.Headers.ContentLength > 0) {
                var json = await response.Content.ReadAsStringAsync();
                so.Value = JToken.Parse(json).ToObject<T>();
            }

            return so;

        }


        public static HttpClientResult<T> Post<T>(this HttpClient client, string relativeUrlFromBase, T obj) {
            return client.PostAsync(relativeUrlFromBase, obj).Result;
        }

        public static async Task<HttpClientResult<T>> PostAsync<T>(
                this HttpClient client, string relativeUrlFromBase, T obj) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new BodyContent<T>(obj)
            };

            var response = await client.SendAsync(msg);

            var so = new HttpClientResult<T> {
                StatusCode = (int)response.StatusCode
            };

            if (so.StatusCode > 299)
                return so;

            if (response.Content.Headers.ContentLength > 0) {
                var json = await response.Content.ReadAsStringAsync();
                if (json != null && json != "")
                    so.Value = JToken.Parse(json).ToObject<T>();
            }

            return so;
        }


        public static HttpClientResult<T> Put<T>(this HttpClient client, string relativeUrlFromBase, T obj) {
            return client.PutAsync(relativeUrlFromBase, obj).Result;
        }


        public static async Task<HttpClientResult<T>> PutAsync<T>(
                this HttpClient client, string relativeUrlFromBase, T obj) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = new Uri(url),
                Content = new BodyContent<T>(obj)
            };

            var response = await client.SendAsync(msg);

            var so = new HttpClientResult<T> {
                StatusCode = (int)response.StatusCode
            };

            if (so.StatusCode > 299)
                return so;

            if (response.Content.Headers.ContentLength > 0) {
                var json = await response.Content.ReadAsStringAsync();
                if (json != null && json != "")
                    so.Value = JToken.Parse(json).ToObject<T>();
            }

            return so;
        }


        public static StatusCodeResult Delete<T>(this HttpClient client, string relativeUrlFromBase, T obj,
            bool flagAsUpdateFirst = false) {
            return client.DeleteAsync(relativeUrlFromBase, obj, flagAsUpdateFirst).Result;
        }


        public static async Task<StatusCodeResult> DeleteAsync<T>(
                this HttpClient client, string relativeUrlFromBase, T obj,
                bool flagAsUpdateFirst = false) {

            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url),
                Content = new BodyContent<T>(obj)
            };
            if (flagAsUpdateFirst)
                msg.Headers.Add("X-PreOperation", "Update");

            var response = await client.SendAsync(msg);

            return new StatusCodeResult((int)response.StatusCode);

        }


        public static StatusCodeResult Delete<T>(this HttpClient client, string relativeUrlFromBase,
                bool flagAsUpdateFirst = false) {
            return client.DeleteAsync<T>(relativeUrlFromBase, flagAsUpdateFirst).Result;
        }


        public static async Task<StatusCodeResult> DeleteAsync<T>(
                this HttpClient client, string relativeUrlFromBase,
                bool flagAsUpdateFirst = false) {

            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url)
            };
            if (flagAsUpdateFirst)
                msg.Headers.Add("X-PreOperation", "Update");

            var response = await client.SendAsync(msg);

            return new StatusCodeResult((int)response.StatusCode);

        }


        public static HttpClientResult<T> Forward<T>(this HttpClient client, HttpRequest request, string relativeUrlFromBase) {
            var msg = request.ToHttpRequestMessage(client);
            return ForwardRequest<T>(client, msg, relativeUrlFromBase);
        }


        public static HttpClientResult<T> Forward<T>(this HttpClient client, HttpRequest request, T body, string relativeUrlFromBase) {
            var msg = request.ToHttpRequestMessage(client, body);
            return ForwardRequest<T>(client, msg, relativeUrlFromBase);
        }




        public static void SendReset(this HttpClient client, string operationName,
            string instanceName) {
            client.SendResetAsync(operationName, instanceName);
        }

        public static async void SendResetAsync(this HttpClient client, string operationName,
            string instanceName) {
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Options,
                RequestUri = client.BaseAddress
            };
            var xTestingHeaders = client.DefaultRequestHeaders.Where(h => h.Key.StartsWith("X-Testing")).Select(h => h.Key).ToArray();
            for (int i = 0; i < xTestingHeaders.Count(); i++)
                client.DefaultRequestHeaders.Remove(xTestingHeaders[i]);

            msg.Headers.Add($"{operationName}", "{instanceName}");
            await client.SendAsync(msg);

        }

        public static bool Ping(this HttpClient client, int timeoutSeconds = 5) {
            return client.PingAsync(timeoutSeconds).Result;
        }


        public static async Task<bool> PingAsync(this HttpClient client, int timeoutSeconds = 5) {

            var pingable = false;

            await Task.Run(() => {

                var port = client.BaseAddress.Port;
                var host = client.BaseAddress.Host;
                var sw = new Stopwatch();

                sw.Start();
                while (sw.ElapsedMilliseconds < (timeoutSeconds * 1000)) {
                    try {
                        using (var tcp = new TcpClient(host, port)) {
                            var connected = tcp.Connected;
                            pingable = true;
                            break;
                        }
                    } catch (Exception ex) {
                        if (!ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                            throw ex;
                        else
                            Thread.Sleep(1000);
                    }

                }

            });
            return pingable;
        }





        private static HttpClientResult<T> ForwardRequest<T>(this HttpClient client, HttpRequestMessage msg, string relativeUrlFromBase) {

            msg.RequestUri = new Url(client.BaseAddress)
                .AppendPathSegment(relativeUrlFromBase)
                .ToUri();

            var response = client.SendAsync(msg).Result;

            var so = new HttpClientResult<T> {
                StatusCode = (int)response.StatusCode
            };

            if (so.StatusCode > 299)
                return so;

            if (response.Content.Headers.ContentLength > 0) {
                var json = response.Content.ReadAsStringAsync().Result;
                so.Value = JToken.Parse(json).ToObject<T>();
            }

            return so;

        }



        private static HttpRequestMessage ToHttpRequestMessage(this HttpRequest httpRequest, HttpClient client) {
            var msg = new HttpRequestMessage();
            msg
                .CopyMethod(httpRequest)
                .CopyHeaders(httpRequest, client)
                .CopyQueryString(httpRequest)
                .CopyCookies(httpRequest);

            return msg;
        }


        private static HttpRequestMessage ToHttpRequestMessage<T>(this HttpRequest httpRequest, HttpClient client, T body) {
            var msg = new HttpRequestMessage();
            msg
                .CopyMethod(httpRequest)
                .CopyHeaders(httpRequest, client)
                .CopyQueryString(httpRequest)
                .CopyCookies(httpRequest);

            var json = JToken.FromObject(body).ToString();
            msg.Content = new StringContent(json,
                    Encoding.UTF8, "application/json");

            return msg;
        }


        private static HttpRequestMessage CopyMethod(this HttpRequestMessage msg, HttpRequest req) {
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


        private static HttpRequestMessage CopyHeaders(this HttpRequestMessage msg, HttpRequest req, HttpClient client) {
            var currentHeaders = client.DefaultRequestHeaders.Select(x => x.Key);
            var requestHeaders = req.Headers.Where(h => !h.Key.StartsWith("Content-"));
            var headers = requestHeaders.Where(h => !currentHeaders.Contains(h.Key));
            foreach (var header in headers)
                msg.Headers.Add(header.Key, header.Value.AsEnumerable());

            return msg;
        }


        private static HttpRequestMessage CopyQueryString(this HttpRequestMessage msg, HttpRequest req) {
            msg.Properties.Add("QueryString", req.QueryString);
            return msg;
        }


        //assumes that HttpClientHandler is set as such:
        //services.AddHttpClient<ColorApiClient>(options => {
        //    options.BaseAddress = new Uri("http://localhost:61006");
        //}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
        //    UseCookies = false
        //});        
        private static HttpRequestMessage CopyCookies(this HttpRequestMessage msg, HttpRequest req) {
            if (req.Cookies != null && req.Cookies.Count > 0) {
                var cookieContainer = new CookieContainer();
                var sb = new StringBuilder();
                foreach (var cookie in req.Cookies) {
                    sb.Append(cookie.Key);
                    sb.Append("=");
                    sb.Append(cookie.Value);
                    sb.Append("; ");
                }
                msg.Headers.Add("Cookie", sb.ToString().TrimEnd());
            }
            return msg;
        }



        //note: this works, but it isn't needed.
        private static HttpRequestMessage CopyContent(this HttpRequestMessage msg, HttpRequest req) {

            var injectedRequestStream = new MemoryStream();

            req.EnableRewind();

            using (var bodyReader = new StreamReader(req.Body)) {
                var bodyAsText = bodyReader.ReadToEnd();
                var msgContent = new StringContent(JToken.FromObject(bodyAsText).ToString(), Encoding.UTF8, "application/json");
                msg.Content = msgContent;

                var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                req.Body = injectedRequestStream;
            }

            return msg;
        }



    }
}

