using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using static EDennis.AspNetCore.Base.Web.TestingActionFilter;


namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This class provides HttpClient extension methods
    /// that support integration testing scenarios.  For
    /// example, a common pattern is to 
    /// (a) POST to create a new record from a provided object
    /// (b) GET to retrieve the created object from the record (to verify the insert)
    /// (c) Drop the testing database
    /// This class provides a method that can accomplish the three
    /// steps above in just one line of code.
    /// </summary>
    public static partial class HttpClientExtensions {

        #region DeleteAndGetForTest

        /// <summary>
        /// DELETEs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the DELETE, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to DELETE and GET</typeparam>
        /// <param name="client">The HttpClient used to DELETE and GET</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> DeleteAndGetMultipleForTest<T>(this HttpClient client, params object[] id) {

            return DeleteAndGetMultipleForTest<T>(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }


        /// <summary>
        /// DELETEs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the DELETE, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to DELETE and GET</typeparam>
        /// <param name="client">The HttpClient used to DELETE and GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a GETed list of objects</returns>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> DeleteAndGetMultipleForTest<T>(this HttpClient client,
            DbType testDbType, string testDb, params object[] id) {

            return DeleteAndGetMultipleForTest<T>(client, testDbType, testDb,
                client.BaseAddress.At(id));
        }

        /// <summary>
        /// DELETEs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <typeparam name="T">The object type to DELETE and GET</typeparam>
        /// <param name="client">The HttpClient used to DELETE and GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="deleteUri">The URL for the DELETE request</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>the GETed object, which should be equivalent to the PUTted object</returns>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> DeleteAndGetMultipleForTest<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri deleteUri, Uri getUri = null, Uri headUri = null) {

            //use the HttpClient's base address as the default for GETs and HEADs
            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the DELETE
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = deleteUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //send the DELETE message
            var response = client.SendAsync(msg).Result;

            //build the request message for the GET
            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //send the GET request
            response = client.SendAsync(msg).Result;

            //deserialize the result and store it
            List<T> content = response.GetObject<List<T>>();

            //build the HEAD message
            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };

            //add a header to request dropping the database 
            msg.DropTestDbHeader(testDbType, testDb);

            //send the HEAD message
            response = client.SendAsync(msg).Result;

            //return the serialized list from the GET request
            return content;
        }

        #endregion
        #region TryDeleteAndGet

        /// <summary>
        /// DELETEs an object at the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the DELETE and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// NOTE: this version of the method assumes that the
        /// test DB type is in-memory and the DB name is the default
        /// </summary>
        /// <typeparam name="T">The object type to DELETE and GET</typeparam>
        /// <param name="client">The HttpClient used to DELETE and GET</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the DELETE and GET</returns>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryDeleteAndGetMultipleForTest<T>(this HttpClient client,
           params object[] id) {

            return TryDeleteAndGetMultipleForTest<T>(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }

        /// <summary>
        /// DELETEs an object at the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the DELETE and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to DELETE and GET</typeparam>
        /// <param name="client">The HttpClient used to DELETE and GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the DELETE and GET</returns>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="DeleteAndGetMultipleForTest{T}(HttpClient, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryDeleteAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryDeleteAndGetMultipleForTest<T>(this HttpClient client,
            DbType testDbType, string testDb, params object[] id) {

            return TryDeleteAndGetMultipleForTest<T>(client, testDbType, testDb,
                client.BaseAddress.At(id), client.BaseAddress, client.BaseAddress);
        }


        /// <summary>
        /// DELETEs an object at the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the DELETE and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to DELETE and GET</typeparam>
        /// <param name="client">The HttpClient used to DELETE and GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="putUri">The URL for the PUT request</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the DELETE and GET</returns>
        public static List<HttpResponseMessage> TryDeleteAndGetMultipleForTest<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri deleteUri, Uri getUri = null, Uri headUri = null) {

            //initialize the to-be-returned list of responses
            var responses = new List<HttpResponseMessage>();

            //use the HttpClient's base address as the default for GETs and HEADs
            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the DELETE
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = deleteUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //send the DELETE message and add the result to the to-be-returned list
            responses.Add(client.SendAsync(msg).Result);

            //build the request message for the DELETE
            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //send the GET request and add the response to the to-be-returned list
            responses.Add(client.SendAsync(msg).Result);

            //build the HEAD message
            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };

            //add a header to request dropping the database 
            msg.DropTestDbHeader(testDbType, testDb);

            //send the HEAD message
            var response = client.SendAsync(msg).Result;

            //return the list of HttpResponseMessage objects
            return responses;
        }

        #endregion

    }


}


