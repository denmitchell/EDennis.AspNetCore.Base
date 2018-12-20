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

        #region Post

        /// <summary>
        /// Performs an asynchronous HTTP POST and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// posted object.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the POST.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <returns>The created object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<T> PostAsync<T>(
            this HttpClient client, T obj) {
            return await PostAsync(client, obj, client.BaseAddress);
        }

        /// <summary>
        /// Performs an asynchronous HTTP POST and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// posted object.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <param name="uri">the URL for the post</param>
        /// <returns>the created object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<T> PostAsync<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //POST the message
            var response = await client.SendAsync(msg);

            //deserialize the result and store it
            T content = await response.GetObjectAsync<T>();

            //return the serialized object
            return content;
        }


        /// <summary>
        /// Performs a synchronous HTTP POST and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// posted object.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the POST.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <returns>the created object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static T Post<T>(
            this HttpClient client, T obj) {
            return Post(client, obj, client.BaseAddress);
        }

        /// <summary>
        /// Performs a synchronous HTTP POST and 
        /// deserializes the return object, which is 
        /// presumed to be of the same type as the 
        /// posted object.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <param name="uri">the URL for the post</param>
        /// <returns>the created object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static T Post<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //POST the message
            var response = client.SendAsync(msg).Result;

            //deserialize the result and store it
            T content = response.GetObject<T>();

            //return the serialized object
            return content;
        }

        #endregion
        #region TryPost

        /// <summary>
        /// Performs an asynchronous HTTP POST and 
        /// returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the POST.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<HttpResponseMessage> TryPostAsync<T>(
            this HttpClient client, T obj) {
            return await TryPostAsync(client, obj, client.BaseAddress);
        }

        /// <summary>
        /// Performs an asynchronous HTTP POST and 
        /// returns the HTTP response message.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <param name="uri">the URL for the post</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static async Task<HttpResponseMessage> TryPostAsync<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //POST the message
            var response = await client.SendAsync(msg);

            //return the response message
            return response;
        }


        /// <summary>
        /// Performs an synchronous HTTP POST and 
        /// returns the HTTP response message.
        /// This overload of the method presumes that
        /// the base URL for the HttpClient is the
        /// exact URL needed for the POST.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static HttpResponseMessage TryPost<T>(
            this HttpClient client, T obj) {
            return TryPost(client, obj, client.BaseAddress);
        }


        /// <summary>
        /// Performs a synchronous HTTP POST and 
        /// returns the HTTP response message.
        /// </summary>
        /// <typeparam name="T">the posted object's type</typeparam>
        /// <param name="client">the HttpClient</param>
        /// <param name="obj">the posted object</param>
        /// <param name="uri">the URL for the post</param>
        /// <returns>the HttpResponseMessage, containing a 
        /// status code and possibly an object</returns>
        /// <seealso cref="PostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="PostAsync{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="Post{T}(HttpClient, T)"/>
        /// <seealso cref="Post{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T)"/>
        /// <seealso cref="TryPost{T}(HttpClient, T, Uri)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T)"/>
        /// <seealso cref="TryPostAsync{T}(HttpClient, T, Uri)"/>
        public static HttpResponseMessage TryPost<T>(
            this HttpClient client, T obj, Uri uri) {

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new BodyContent<T>(obj)
            };

            //POST the message
            var response = client.SendAsync(msg).Result;

            //return the response message
            return response;
        }


        #endregion
    }
}

