using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
        public static async Task<T> GetAsync<T>(
            this HttpClient client, params object[] id) {
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
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
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
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
        public static T Get<T>(
            this HttpClient client, params object[] id) {
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
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
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

        /// <summary>
        /// Performs a synchronous GET for a list of objects.
        /// NOTE: this version of the method assumes that the 
        /// GET url is the base url for the HttpClient object.
        /// </summary>
        /// <typeparam name="T">The type of each element in the list</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
        public static List<T> GetMultiple<T>(this HttpClient client) {
            return GetMultiple<T>(client, client.BaseAddress);
        }

        /// <summary>
        /// Performs a synchronous GET for a list of objects
        /// </summary>
        /// <typeparam name="T">The type of each element in the list</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
        public static List<T> GetMultiple<T>(this HttpClient client,
            Uri getUri = null) {

            //use the client's base address as the default address
            if (getUri == null)
                getUri = client.BaseAddress;

            //build a new request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //send the GET request.
            var response = client.SendAsync(msg).Result;

            //retrieve and deserialize the list of objects
            List<T> content = response.GetObject<List<T>>();

            //return the list of objects
            return content;
        }

        /// <summary>
        /// Performs an asynchronous GET for a list of objects.
        /// NOTE: this version of the method assumes that the 
        /// GET url is the base url for the HttpClient object.
        /// </summary>
        /// <typeparam name="T">The type of each element in the list</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
        public async static Task<List<T>> GetMultipleAsync<T>(this HttpClient client) {
            return await GetMultipleAsync<T>(client, client.BaseAddress);
        }

        /// <summary>
        /// Performs a synchronous GET for a list of objects
        /// </summary>
        /// <typeparam name="T">The type of each element in the list</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet{T}(HttpClient, object[])"/>
        /// <see cref="TryGet{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync{T}(HttpClient, object[])"/>
        /// <see cref="TryGetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient)"/>
        /// <see cref="TryGetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync{T}(HttpClient, Uri)"/>
        public async static Task<List<T>> GetMultipleAsync<T>(this HttpClient client,
            Uri getUri = null) {

            //use the client's base address as the default address
            if (getUri == null)
                getUri = client.BaseAddress;

            //build a new request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //send the GET request.
            var response = await client.SendAsync(msg);

            //retrieve and deserialize the list of objects
            List<T> content = await response.GetObjectAsync<List<T>>();

            //return the list of objects
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
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryGetAsync(
            this HttpClient client, params object[] id) {
            return await TryGetAsync(client,
                client.BaseAddress.At(id));
        }

        /// <summary>
        /// Performs an asynchronous HTTP GET
        /// and returns the HTTP response message.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public static async Task<HttpResponseMessage> TryGetAsync(
            this HttpClient client, Uri uri) {

            //build a new request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            //send the message and receive the response
            var response = await client.SendAsync(msg);

            //return the HttpResponseMessage object
            return response;
        }

        /// <summary>
        /// Performs an synchronous HTTP GET
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
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryGet(
            this HttpClient client, params object[] id) {
            return TryGet(client,
                client.BaseAddress.At(id));
        }

        /// <summary>
        /// Performs an synchronous HTTP GET
        /// and returns the HTTP response message.
        /// </summary>
        /// <param name="client">the HttpClient</param>
        /// <param name="uri">the URL for the GET request</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryGet(
            this HttpClient client, Uri uri) {

            //build a new request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            //send the message and receive the response
            var response = client.SendAsync(msg).Result;

            //return the response
            return response;
        }

        /// <summary>
        /// Performs an asynchronous GET for a list of objects, but
        /// returns the HttpResponseMessage
        /// NOTE: this version of the method assumes that the 
        /// GET url is the base url for the HttpClient object.
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <returns>the HttpResponseMessage, which contains the
        /// status code and may contain a list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public async static Task<HttpResponseMessage> TryGetMultipleAsync(this HttpClient client) {
            return await TryGetMultipleAsync(client, client.BaseAddress);
        }

        /// <summary>
        /// Performs a synchronous GET for a list of objects, but
        /// returns the HttpResponseMessage
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <returns>the HttpResponseMessage, which contains the
        /// status code and may contain a list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public async static Task<HttpResponseMessage> TryGetMultipleAsync(
            this HttpClient client, Uri getUri = null) {

            //use the client's base address as the default address
            if (getUri == null)
                getUri = client.BaseAddress;

            //build a new request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //send the GET request.
            var response = await client.SendAsync(msg);

            //return the HttpResponseMessage object
            return response;
        }

        /// <summary>
        /// Performs a synchronous GET for a list of objects, but
        /// returns the HttpResponseMessage
        /// NOTE: this version of the method assumes that the 
        /// GET url is the base url for the HttpClient object.
        /// </summary>
        /// <typeparam name="T">The type of each element in the list</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <returns>the HttpResponseMessage, which contains the
        /// status code and may contain a list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryGetMultiple(this HttpClient client) {
            return TryGetMultiple(client, client.BaseAddress);
        }

        /// <summary>
        /// Performs a synchronous GET for a list of objects, but
        /// returns the HttpResponseMessage
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <returns>the HttpResponseMessage, which contains the
        /// status code and may contain a list of objects</returns>
        /// <see cref="Get{T}(HttpClient, object[])"/>
        /// <see cref="Get{T}(HttpClient, Uri)"/>
        /// <see cref="GetAsync{T}(HttpClient, object[])"/>
        /// <see cref="GetAsync{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultiple{T}(HttpClient)"/>
        /// <see cref="GetMultiple{T}(HttpClient, Uri)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient)"/>
        /// <see cref="GetMultipleAsync{T}(HttpClient, Uri)"/>
        /// <see cref="TryGet(HttpClient, object[])"/>
        /// <see cref="TryGet(HttpClient, Uri)"/>
        /// <see cref="TryGetAsync(HttpClient, object[])"/>
        /// <see cref="TryGetAsync(HttpClient, Uri)"/>
        /// <see cref="TryGetMultiple(HttpClient)"/>
        /// <see cref="TryGetMultiple(HttpClient, Uri)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient)"/>
        /// <see cref="TryGetMultipleAsync(HttpClient, Uri)"/>
        public static HttpResponseMessage TryGetMultiple(
            this HttpClient client, Uri getUri = null) {

            //use the client's base address as the default address
            if (getUri == null)
                getUri = client.BaseAddress;

            //build a new request message object
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //send the GET request.
            var response = client.SendAsync(msg).Result;

            //return the HttpResponseMessage object
            return response;
        }


        #endregion


    }

}

