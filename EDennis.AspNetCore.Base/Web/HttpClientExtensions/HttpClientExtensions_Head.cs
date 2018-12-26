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
    /// for sending POST, PUT, GET, and HEAD message and
    /// deserializing the response bodies.  These methods 
    /// are convenience methods that should result in fewer
    /// lines of code required for requests using HttpClient.
    /// </summary>
    public static partial class HttpClientExtensions {

        #region Head

        /// <summary>
        /// Performs an asynchronous HTTP HEAD.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the HEAD, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static async Task HeadAsync(this HttpClient client) {
            await HeadAsync(client,client.BaseAddress);
        }


        /// <summary>
        /// Performs an asynchronous HTTP HEAD.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static async Task HeadAsync(this HttpClient client, Uri uri) {

            //build the request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = uri
            };

            //send the message and get the response
            var response = await client.SendAsync(msg);

            //return
            return;
        }


        /// <summary>
        /// Performs a synchronous HTTP HEAD.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the HEAD, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static void Head(this HttpClient client) {
            Head(client, client.BaseAddress);
        }


        /// <summary>
        /// Performs a synchronous HTTP HEAD.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static void Head(
            this HttpClient client, Uri uri) {

            //build the request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = uri
            };

            //send the message and get the response
            var response = client.SendAsync(msg).Result;
        }

        #endregion
        #region TryHead

        /// <summary>
        /// Performs an asynchronous HTTP HEAD
        /// and returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the HEAD, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryHeadAsync(
            this HttpClient client) {
            return await TryHeadAsync(client, client.BaseAddress);
        }


        /// <summary>
        /// Performs an asynchronous HTTP HEAD
        /// and returns the HTTP response message.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryHeadAsync(
            this HttpClient client, Uri uri) {

            //build the request message
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = uri
            };

            //send the message and get the response
            var response = await client.SendAsync(msg);

            //return the response
            return response;
        }


        /// <summary>
        /// Performs a synchronous HTTP HEAD
        /// and returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the HEAD, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryHead(
            this HttpClient client) {
            return TryHead(client, client.BaseAddress);
        }


        /// <summary>
        /// Performs a synchronous HTTP HEAD
        /// and returns the HTTP response message.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="uri">the url for the delete</param>
        /// <seealso cref="HeadAsync(HttpClient, object[])"/>
        /// <seealso cref="HeadAsync(HttpClient, Uri)"/>
        /// <seealso cref="Head(HttpClient, object[])"/>
        /// <seealso cref="Head(HttpClient, Uri)"/>
        /// <seealso cref="TryHead(HttpClient, object[])"/>
        /// <seealso cref="TryHead(HttpClient, Uri)"/>
        /// <seealso cref="TryHeadAsync(HttpClient, object[])"/>
        /// <seealso cref="TryHeadAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryHead(
            this HttpClient client, Uri uri) {

            //build the request message
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = uri
            };

            //send the message and get the response
            var response = client.SendAsync(msg).Result;

            //return the response
            return response;
        }

        #endregion
    }
}

