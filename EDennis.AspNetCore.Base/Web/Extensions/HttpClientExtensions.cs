using Flurl;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.AspNetCore.Base.Web {
    public static class HttpClientExtensions {

        public static ObjectResult<T> Get<T>(this HttpClient client, string relativeUrlFromBase) {
            return client.GetAsync<T>(relativeUrlFromBase).Result;
        }

        public static async Task<ObjectResult<T>> GetAsync<T>(
                this HttpClient client, string relativeUrlFromBase) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);
            var response = await client.GetAsync(url);

            var so = new ObjectResult<T> {
                StatusCode = response.StatusCode
            };

            if (so.StatusCodeValue > 299)
                return so;

            var json = await response.Content.ReadAsStringAsync();
            so.Value = JToken.Parse(json).ToObject<T>();

            return so;

        }


        public static ObjectResult<T> Post<T>(this HttpClient client, string relativeUrlFromBase, T obj) {
            return client.PostAsync(relativeUrlFromBase, obj).Result;
        }

        public static async Task<ObjectResult<T>> PostAsync<T>(
                this HttpClient client, string relativeUrlFromBase, T obj) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new BodyContent<T>(obj)
            };

            var response = await client.SendAsync(msg);

            var so = new ObjectResult<T>() {
                StatusCode = response.StatusCode
            };

            if (so.StatusCodeValue > 299)
                return so;

            if (response.Content != null) {
                var json = await response.Content.ReadAsStringAsync();
                if (json != null && json != "")
                    so.Value = JToken.Parse(json).ToObject<T>();
            }

            return so;
        }


        public static ObjectResult<T> Put<T>(this HttpClient client, string relativeUrlFromBase, T obj) {
            return client.PutAsync(relativeUrlFromBase, obj).Result;
        }


        public static async Task<ObjectResult<T>> PutAsync<T>(
                this HttpClient client, string relativeUrlFromBase, T obj) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = new Uri(url),
                Content = new BodyContent<T>(obj)
            };

            var response = await client.SendAsync(msg);

            var so = new ObjectResult<T> {
                StatusCode = response.StatusCode
            };

            if (so.StatusCodeValue > 299)
                return so;

            if (response.Content != null) {
                var json = await response.Content.ReadAsStringAsync();
                if (json != null && json != "")
                    so.Value = JToken.Parse(json).ToObject<T>();
            }

            return so;
        }


        public static HttpStatusCode Delete<T>(this HttpClient client, string relativeUrlFromBase, T obj,
            bool flagAsUpdateFirst = false) {
            return client.DeleteAsync(relativeUrlFromBase, obj, flagAsUpdateFirst).Result;
        }


        public static async Task<HttpStatusCode> DeleteAsync<T>(
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

            return response.StatusCode;

        }


        public static HttpStatusCode Delete<T>(this HttpClient client, string relativeUrlFromBase,
                bool flagAsUpdateFirst = false) {
            return client.DeleteAsync<T>(relativeUrlFromBase, flagAsUpdateFirst).Result;
        }


        public static async Task<HttpStatusCode> DeleteAsync<T>(
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

            return response.StatusCode;

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
            var xTestingHeaders = client.DefaultRequestHeaders.Where(h => h.Key.StartsWith(Interceptor.HDR_PREFIX)).Select(h=>h.Key).ToArray();
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

    }
}
