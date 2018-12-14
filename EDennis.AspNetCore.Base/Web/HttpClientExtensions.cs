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
    public static class HttpClientExtensions {

        public static string GetControllerUrl(this IApiProxy proxy, IConfiguration config, string controllerName) {
            var apiName = proxy.GetType().Name;
            var url = config[$"Apis:{apiName}:ControllerUrls:{controllerName}"];
            return url;
        }


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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(msg);
            T content = await response.Content.ReadAsAsync<T>();
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = client.SendAsync(msg).Result;
            T content = response.Content.ReadAsAsync<T>().Result;
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(msg);
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = client.SendAsync(msg).Result;
            return response;
        }


        #endregion
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(msg);
            T content = await response.Content.ReadAsAsync<T>();
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = client.SendAsync(msg).Result;
            T content = response.Content.ReadAsAsync<T>().Result;
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(msg);
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
            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = client.SendAsync(msg).Result;
            return response;
        }

        #endregion
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

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            var response = await client.SendAsync(msg);
            T content = await response.Content.ReadAsAsync<T>();
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

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            var response = client.SendAsync(msg).Result;
            T content = response.Content.ReadAsAsync<T>().Result;
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
        #region TrySend

        public static List<HttpResponseMessage> TrySend(
            this HttpClient client, List<HttpRequestMessage> messages) {

            var responses = new List<HttpResponseMessage>();

            foreach (var message in messages)
                responses.Add(client.SendAsync(message).Result);

            return responses;
        }

        #endregion
        #region Helper Methods


        private static void UseTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            if (testDbType == DbType.Transaction ||
                testDbType == DbType.InMemory) {

                var key = HDR_USE_INMEMORY;
                if (testDbType == DbType.Transaction)
                    key = HDR_USE_TRANSACTION;
                msg.Headers.Add(key, testDb);
            }
        }

        private static void DropTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            if (testDbType == DbType.Transaction ||
                testDbType == DbType.InMemory) {

                var key = HDR_DROP_INMEMORY;
                if (testDbType == DbType.Transaction)
                    key = HDR_ROLLBACK_TRANSACTION;
                msg.Headers.Add(key, testDb);
            }
        }

        #endregion


    }

    public static class HttpResponseMessageExtensions {
        public static T GetObject<T>(this HttpResponseMessage message) {
            var json = message.Content.ReadAsStringAsync().Result;
            return JToken.Parse(json).ToObject<T>();
        }
        public static async Task<T> GetObjectAsync<T>(this HttpResponseMessage message) {
            var json = await message.Content.ReadAsStringAsync();
            return JToken.Parse(json).ToObject<T>();
        }
    }


    public static class UriExtensions {

        public static Uri At(this Uri uri, params object[] id) {

            var uriParts = uri.PathAndQuery.Split('?');

            var sb = new StringBuilder();
            foreach (var val in id) {
                sb.Append("/");
                sb.Append(ToFormattedString(val));
            }

            var newUri = TrimTrailingSlash(uriParts[0]) + sb.ToString();
            if (uriParts.Length > 1)
                newUri += "?" + uriParts[1];

            UriKind uriKind = UriKind.Relative;
            if (newUri.StartsWith("http"))
                uriKind = UriKind.Absolute;

            return new Uri(newUri,uriKind);
        }

        public static Uri Where(this Uri uri, string key, object value) {
            var uriParts = uri.PathAndQuery.Split('?');
            var separator = "?";
            if (uriParts.Length > 1)
                separator = "&";

            UriKind uriKind = UriKind.Relative;
            if (uriParts[0].StartsWith("http"))
                uriKind = UriKind.Absolute;

            return new Uri(uriParts[0] + $"{separator}{key}={ToFormattedString(value)}", uriKind);
        }

        public static string ToFormattedString(object value) {
            if (value is DateTime val) {
                if (val.Hour > 0 || val.Minute > 0 || val.Second > 0) {
                    return val.ToString("yyyyMMddTHHmmss");
                } else {
                    return val.ToString("yyyyMMdd");
                }
            } else if (value is bool) {
                return (bool)value ? "true" : "false";
            } else
                return value.ToString();
        }

        private static string TrimTrailingSlash(string str) {
            if (str.EndsWith("/") || str.EndsWith("\\"))
                str = str.Substring(0, str.Length - 1);
            return str;
        }

    }

}

