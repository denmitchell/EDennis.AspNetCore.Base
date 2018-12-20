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

        #region Get

        /// <summary>
        /// Performs an asynchronous HTTP GET
        /// and returns a deserialized object.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the GET, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the retrieved object</returns>
        /// <seealso cref="GetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <seealso cref="Get{T}(HttpClient, object[])"/>
        /// <seealso cref="Get{T}(HttpClient, Uri)"/>
        /// <seealso cref="GetMultiple{T}(HttpClient, DbType, string, Uri)"/>
        /// <seealso cref="TryGet{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGet{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, Uri)"/>
        public static async Task<T> GetAsync<T>(
            this HttpClient client,
            params object[] id) {
            return await GetAsync<T>(client,
                client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs an asynchronous HTTP GET
        /// and returns a deserialized object.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the retrieved object</returns>
        /// <seealso cref="GetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <seealso cref="Get{T}(HttpClient, object[])"/>
        /// <seealso cref="Get{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGet{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGet{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, Uri)"/>
        public static async Task<T> GetAsync<T>(
            this HttpClient client, Uri uri) {

            //build the request message object for the GET
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            //send the GET message
            var response = await client.SendAsync(msg);

            //deserialize the result and store it
            T content = await response.GetObjectAsync<T>();

            //return the serialized object
            return content;
        }


        /// <summary>
        /// Performs an synchronous HTTP GET
        /// and returns a deserialized object.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the GET, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the retrieved object</returns>
        /// <seealso cref="GetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <seealso cref="Get{T}(HttpClient, object[])"/>
        /// <seealso cref="Get{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGet{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGet{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, Uri)"/>
        public static T Get<T>(
            this HttpClient client,
            params object[] id) {
            return Get<T>(client,
                client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs a synchronous HTTP GET
        /// and returns a deserialized object.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the retrieved object</returns>
        /// <seealso cref="GetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <seealso cref="Get{T}(HttpClient, object[])"/>
        /// <seealso cref="Get{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGet{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGet{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, Uri)"/>
        public static T Get<T>(
            this HttpClient client, Uri uri) {

            //build the request message object for the GET
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            
            //send the GET message
            var response = client.SendAsync(msg).Result;

            //deserialize the result and store it
            T content = response.Content.ReadAsAsync<T>().Result;

            //return the serialized object
            return content;
        }


        public static List<T> GetMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri getUri = null) {

            if (getUri == null)
                getUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;
            List<T> content = response.Content.ReadAsAsync<List<T>>().Result;

            return content;
        }


        #endregion
        #region TryGet

        /// <summary>
        /// Performs an asynchronous HTTP GET
        /// and returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the GET, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="GetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <seealso cref="Get{T}(HttpClient, object[])"/>
        /// <seealso cref="Get{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGet{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGet{T}(HttpClient, Uri)"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <seealso cref="TryGetAsync{T}(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryGetAsync<T>(
            this HttpClient client,
            params object[] id) {
            return await TryGetAsync<T>(client,
                client.BaseAddress.At(id));
        }

        public static async Task<HttpResponseMessage> TryGetAsync<T>(
            this HttpClient client, Uri uri) {

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            var response = await client.SendAsync(msg);
            return response;
        }


        public static HttpResponseMessage TryGet<T>(
            this HttpClient client,
            params object[] id) {
            return TryGet<T>(client,
                client.BaseAddress.At(id));
        }

        public static HttpResponseMessage TryGet<T>(
            this HttpClient client, Uri uri) {

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            var response = client.SendAsync(msg).Result;
            return response;
        }


        public static HttpResponseMessage TryGetMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri getUri = null) {

            if (getUri == null)
                getUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;
            return response;
        }


        #endregion


    }

}

