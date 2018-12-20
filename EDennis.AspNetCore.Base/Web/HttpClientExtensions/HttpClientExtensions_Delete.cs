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

    /// <summary>
    /// This class provides a number of extension methods
    /// for sending POST, PUT, GET, and DELETE message and
    /// deserializing the response bodies.  These methods 
    /// are convenience methods that should result in fewer
    /// lines of code required for requests using HttpClient.
    /// </summary>
    public static partial class HttpClientExtensions {

        #region Delete

        /// <summary>
        /// Performs an asynchronous HTTP DELETE.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the DELETE, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static async Task DeleteAsync(
            this HttpClient client,
            params object[] id) {
            await DeleteAsync(client,
                client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs an asynchronous HTTP DELETE.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static async Task DeleteAsync(
            this HttpClient client, Uri uri) {

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            var response = await client.SendAsync(msg);
            return;
        }


        /// <summary>
        /// Performs a synchronous HTTP DELETE.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the DELETE, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static void Delete(
            this HttpClient client,
            params object[] id) {
            Delete(client, client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs a synchronous HTTP DELETE.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static void Delete(
            this HttpClient client, Uri uri) {
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            var response = client.SendAsync(msg).Result;
        }

        #endregion
        #region TryDelete

        /// <summary>
        /// Performs an asynchronous HTTP DELETE
        /// and returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the DELETE, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryDeleteAsync(
            this HttpClient client,
            params object[] id) {
            return await TryDeleteAsync(client,
                client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs an asynchronous HTTP DELETE
        /// and returns the HTTP response message.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryDeleteAsync(
            this HttpClient client, Uri uri) {

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            var response = await client.SendAsync(msg);
            return response;
        }


        /// <summary>
        /// Performs a synchronous HTTP DELETE
        /// and returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the DELETE, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryDelete(
            this HttpClient client,
            params object[] id) {
            return TryDelete(client, client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs a synchronous HTTP DELETE
        /// and returns the HTTP response message.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="DeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="DeleteAsync(HttpClient, Uri)"/>
        /// <seealso cref="Delete(HttpClient, object[])"/>
        /// <seealso cref="Delete(HttpClient, Uri)"/>
        /// <seealso cref="TryDelete(HttpClient, object[])"/>
        /// <seealso cref="TryDelete(HttpClient, Uri)"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, object[])"/>
        /// <seealso cref="TryDeleteAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryDelete(
            this HttpClient client, Uri uri) {
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            var response = client.SendAsync(msg).Result;
            return response;
        }

        #endregion
    }
}

