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

        #region Put

        /// <summary>
        /// Performs an asynchronous HTTP PUT and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// put object.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the PUT, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>The updated object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<T> PutAsync<T>(
            this HttpClient client, T obj,
            params object[] id) {
            return await PutAsync(client, obj, 
                client.BaseAddress.At(id));
        }

        /// <summary>
        /// Performs an asynchronous HTTP PUT and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// put object.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="uri">the url for the put</param>
        /// <returns>The updated object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<T> PutAsync<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //PUT the message
            var response = await client.SendAsync(msg);

            //deserialize the result and store it
            T content = await response.GetObjectAsync<T>();

            //return the serialized object
            return content;
        }

        /// <summary>
        /// Performs a synchronous HTTP PUT and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// put object.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the PUT, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>The updated object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static T Put<T>(
            this HttpClient client, T obj,
            params object[] id) {
            return Put(client, obj, client.BaseAddress.At(id));
        }

        /// <summary>
        /// Performs a synchronous HTTP PUT and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// put object.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="uri">the url for the put</param>
        /// <returns>The updated object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static T Put<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //PUT the message
            var response = client.SendAsync(msg).Result;

            //deserialize the result and store it
            T content = response.GetObject<T>();

            //return the serialized object
            return content;
        }

        #endregion
        #region TryPut

        /// <summary>
        /// Performs an asynchronous HTTP PUT and 
        /// returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the PUT, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<HttpResponseMessage> TryPutAsync<T>(
            this HttpClient client, T obj,
            params object[] id) {
            return await TryPutAsync(client, obj,
                client.BaseAddress.At(id));
        }


        /// <summary>
        /// Performs an asynchronous HTTP PUT and 
        /// returns the HTTP response message.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="uri">the url for the put</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<HttpResponseMessage> TryPutAsync<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //PUT the message
            var response = await client.SendAsync(msg);

            //return the response message
            return response;
        }


        /// <summary>
        /// Performs a synchronous HTTP PUT and 
        /// returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the PUT, except for
        /// the addition of the id in the path.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="id">the param array of values representing the Id</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static HttpResponseMessage TryPut<T>(
            this HttpClient client, T obj,
            params object[] id) {
            return TryPut(client, obj, client.BaseAddress.At(id));
        }

        /// <summary>
        /// Performs an asynchronous HTTP PUT and 
        /// returns the HTTP response message.
        /// </summary>
        /// <typeparam name="T">the put object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the put object</param>
        /// <param name="uri">the url for the put</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="PutAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Put{T}(HttpClient, T, object[])"/>
        /// <seealso cref="Put{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPut{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, object[])"/>
        /// <seealso cref="TryPutAsync{T}(HttpClient, T, Uri)"/>
        public static HttpResponseMessage TryPut<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //PUT the message
            var response = client.SendAsync(msg).Result;

            //return the response message
            return response;
        }

        #endregion

    }

}

