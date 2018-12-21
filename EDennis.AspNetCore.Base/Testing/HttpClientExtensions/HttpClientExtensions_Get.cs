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

        #region GetForTest

        /// <summary>
        /// Sends a Get request, retrieves the result,
        /// disposes the test database, and returns the result.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to GET</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>The GETed object</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static T GetForTest<T>(this HttpClient client, params object[] id) {
            return GetForTest<T>(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }

        /// <summary>
        /// Sends a Get request, retrieves the result,
        /// disposes the test database, and returns the result.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to GET</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>The GETed object</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static T GetForTest<T>(this HttpClient client,
            DbType testDbType, string testDb, params object[] id) {
            Uri getUri = client.BaseAddress.At(id);
            return GetForTest<T>(client, testDbType, testDb, 
                getUri, client.BaseAddress);
        }

        /// <summary>
        /// Sends a Get request, retrieves the result,
        /// disposes the test database, and returns the result.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <typeparam name="T">The object type to GET</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>The GETed object</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static T GetForTest<T>(this HttpClient client,
            DbType testDbType, string testDb, Uri getUri, Uri headUri = null) {


            //use the HttpClient's base address as the default for HEADs
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the GET
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //send the GET request
            var response = client.SendAsync(msg).Result;

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
        /// Sends a Get request for a list of objects, 
        /// retrieves the result, disposes the test database, 
        /// and returns the result.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to GET</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <returns>The GETed list of objects</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static List<T> GetForTestMultiple<T>(this HttpClient client) {
            return GetForTestMultiple<T>(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// Sends a Get request for a list of objects, 
        /// retrieves the result, disposes the test database, 
        /// and returns the result.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to GET</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <returns>The GETed list of objects</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static List<T> GetForTestMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb) {
            return GetForTestMultiple<T>(client, testDbType, testDb,
                client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// Sends a Get request for a list of objects, 
        /// retrieves the result, disposes the test database, 
        /// and returns the result.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <typeparam name="T">The object type to GET</typeparam>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>The GETed list of objects</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static List<T> GetForTestMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri getUri, Uri headUri = null) {

            //use the HttpClient's base address as the default for HEAD
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the GET
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //GET the message
            var response = client.SendAsync(msg).Result;

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
        #region TryGetForTest


        /// <summary>
        /// Sends a Get request, retrieves the result,
        /// disposes the test database, and returns the 
        /// HttpResponseMessage, which includes the status code.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="id">The ID of the object to GET</param>
        /// <returns>The HttpResponseMessage, which contains the
        /// status code and may also contain the GETed object</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static HttpResponseMessage TryGetForTest(this HttpClient client,
            params object[] id) {
            Uri getUri = client.BaseAddress.At(id);
            return TryGetForTest(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }


        /// <summary>
        /// Sends a Get request, retrieves the result,
        /// disposes the test database, and returns the 
        /// HttpResponseMessage, which includes the status code.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="id">The ID of the object to GET</param>
        /// <returns>The HttpResponseMessage, which contains the
        /// status code and may also contain the GETed object</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static HttpResponseMessage TryGetForTest(this HttpClient client,
            DbType testDbType, string testDb, params object[] id) {
            Uri getUri = client.BaseAddress.At(id);
            return TryGetForTest(client, testDbType, testDb,
                getUri, getUri, client.BaseAddress);
        }

        /// <summary>
        /// Sends a Get request, retrieves the result,
        /// disposes the test database, and returns the 
        /// HttpResponseMessage, which includes the status code.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>The HttpResponseMessage, which contains the
        /// status code and may also contain the GETed object</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static HttpResponseMessage TryGetForTest(this HttpClient client,
            DbType testDbType, string testDb,
            Uri getUri, Uri headUri = null) {

            //use the HttpClient's base address as the default for HEADs
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the GET
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //GET the message and add the result to the to-be-returned list
            var response = client.SendAsync(msg).Result;

            //build the X-Testing- header
            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };

            //add a header to request dropping the database 
            msg.DropTestDbHeader(testDbType, testDb);

            //send the HEAD message
            var responseHead = client.SendAsync(msg).Result;

            //return the HttpResponseMessage object
            return response;
        }


        /// <summary>
        /// Sends a Get request for a list of objects, 
        /// retrieves the result, disposes the test database, 
        /// and returns the HttpResponseMessage, which includes 
        /// the status code.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <returns>The HttpResponseMessage, which contains the
        /// status code and may also contain the GETed list of objects</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static HttpResponseMessage TryGetForTestMultiple(this HttpClient client) {
            return TryGetForTestMultiple(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// Sends a Get request for a list of objects, 
        /// retrieves the result, disposes the test database, 
        /// and returns the HttpResponseMessage, which includes 
        /// the status code.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <returns>The HttpResponseMessage, which contains the
        /// status code and may also contain the GETed list of objects</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static HttpResponseMessage TryGetForTestMultiple(this HttpClient client,
            DbType testDbType, string testDb) {

            return TryGetForTestMultiple(client, testDbType, testDb,
                client.BaseAddress, client.BaseAddress);
        }


        /// <summary>
        /// Sends a Get request for a list of objects, 
        /// retrieves the result, disposes the test database, 
        /// and returns the HttpResponseMessage, which includes 
        /// the status code.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <param name="client">The HttpClient used to GET</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>The HttpResponseMessage, which contains the
        /// status code and may also contain the GETed list of objects</returns>
        /// <see cref="GetForTest{T}(HttpClient, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, object[])"/>
        /// <see cref="GetForTest{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string)"/>
        /// <see cref="GetForTestMultiple{T}(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTest(HttpClient, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, object[])"/>
        /// <see cref="TryGetForTest(HttpClient, DbType, string, Uri, Uri)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string)"/>
        /// <see cref="TryGetForTestMultiple(HttpClient, DbType, string, Uri, Uri)"/>
        public static HttpResponseMessage TryGetForTestMultiple(this HttpClient client,
            DbType testDbType, string testDb, Uri getUri, Uri headUri = null) {

            //use the HttpClient's base address as the default for HEAD
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the GET
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //GET the message and add the result to the to-be-returned list
            var response = client.SendAsync(msg).Result;

            //build the HEAD message
            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };

            //add a header to request dropping the database 
            msg.DropTestDbHeader(testDbType, testDb);

            //send the HEAD message
            var response2 = client.SendAsync(msg).Result;

            //return the HttpResponseMessage object
            return response;
        }

        #endregion

    }

}


