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

        #region PutAndGetForTest

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetForTest uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the PUTted object</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PutAndGetForTest<T>(this HttpClient client,
            T obj, params object[] id) {
            return PutAndGetForTest(client, obj, DbType.InMemory,
                DEFAULT_NAMED_INSTANCE, id);
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetForTest uses an 
        /// in-memory database with a default name
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the PUTted object</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PutAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {
            Uri putAndGetUri = client.BaseAddress.At(id);
            return PutAndGetForTest(client, obj, testDbType, testDb,
                putAndGetUri, putAndGetUri, client.BaseAddress);
        }

        /// <summary>
        /// PUTs an object to the designated URL,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <param name="putUri">The URL for the PUT request</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>the GETed object, which should be equivalent to the PUTted object</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PutAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            //by default, assume that the GET URL is the same as the POST URL
            if (getUri == null)
                getUri = putUri;

            //use the HttpClient's base address as the default for HEADs
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //PUT the message
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
            T content = response.GetObject<T>();

            //build the HEAD message
            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };

            //add a header to request dropping the database 
            msg.DropTestDbHeader(testDbType, testDb);

            //send the HEAD message
            response = client.SendAsync(msg).Result;

            //return the serialized object from the GET request
            return content;
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetMultipleForTest uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the PUT, GET, and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PutAndGetMultipleForTest<T>(this HttpClient client, 
            T obj, params object[] id) {

            return PutAndGetMultipleForTest(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress.At(id), client.BaseAddress, client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetMultipleForTest uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the PUT and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri"></param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns></returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PutAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb, params object[] id) {

            return PutAndGetMultipleForTest(client, obj, testDbType, testDb,
                client.BaseAddress.At(id), client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// PUTs an object to the designated URL,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetForTest (a) can use either
        /// an in-memory database or a database with an
        /// open transaction that can be rolled back and (b)
        /// explicitly specifies the POST, GET, and HEAD URLs.
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PutAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            //use the HttpClient's base address as the default for GETs and HEADs
            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //PUT the message
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
        #region TryPutAndGet


        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the PUT and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the PUT and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the PUT and GET</returns>
        public static List<HttpResponseMessage> TryPutAndGetForTest<T>(this HttpClient client,
            T obj, params object[] id) {
            Uri putUri = client.BaseAddress.At(id);
            return TryPutAndGetForTest(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }


        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the PUT and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the PUT and GET</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPutAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {
            Uri putUri = client.BaseAddress.At(id);
            return TryPutAndGetForTest(client, obj, testDbType, testDb,
                putUri, putUri, client.BaseAddress);
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the PUT and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the PUT and GET</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPutAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            //initialize the to-be-returned list of responses
            var responses = new List<HttpResponseMessage>();

            //use the PUT url as the default for GET
            if (getUri == null)
                getUri = putUri;

            //use the HttpClient's base address as the default for HEADs
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //PUT the message and add the result to the to-be-returned list
            responses.Add(client.SendAsync(msg).Result);

            //build the request message for the GET
            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //send the GET request and add the response to the to-be-returned list
            responses.Add(client.SendAsync(msg).Result);

            //build the X-Testing- header
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


        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the PUT and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// NOTE: this version of the method assumes that the
        /// test DB type is in-memory and the DB name is the default
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the PUT and GET</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPutAndGetMultipleForTest<T>(this HttpClient client,
            T obj, params object[] id) {

            return TryPutAndGetMultipleForTest(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress.At(id), client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the PUT and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the PUT and GET</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPutAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb, params object[] id) {

            return TryPutAndGetMultipleForTest(client, obj, testDbType, testDb,
                client.BaseAddress.At(id), client.BaseAddress, client.BaseAddress);
        }


        /// <summary>
        /// PUTs an object to the designated URL,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the PUT and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the PUT, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="putUri">The URL for the PUT request</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the PUT and GET</returns>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPutAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            //initialize the to-be-returned list of responses
            var responses = new List<HttpResponseMessage>();

            //use the HttpClient's base address as the default for GETs and HEADs
            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the PUT
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //PUT the message and add the result to the to-be-returned list
            responses.Add(client.SendAsync(msg).Result);

            //build the request message for the GET
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


