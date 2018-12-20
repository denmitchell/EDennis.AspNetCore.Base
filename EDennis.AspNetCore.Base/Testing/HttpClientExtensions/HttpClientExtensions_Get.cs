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

        public static T GetForTest<T>(this HttpClient client, params object[] id) {
            return GetForTest<T>(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }

        public static T GetForTest<T>(this HttpClient client,
            DbType testDbType, string testDb, params object[] id) {
            Uri getUri = client.BaseAddress.At(id);
            return GetForTest<T>(client, testDbType, testDb, 
                getUri, client.BaseAddress);
        }

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

        public static List<T> GetForTestMultiple<T>(this HttpClient client) {
            return GetForTestMultiple<T>(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress, client.BaseAddress);
        }

        public static List<T> GetForTestMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb) {
            return GetForTestMultiple<T>(client, testDbType, testDb,
                client.BaseAddress, client.BaseAddress);
        }

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


        public static HttpResponseMessage TryGetForTest(this HttpClient client,
            params object[] id) {
            Uri getUri = client.BaseAddress.At(id);
            return TryGetForTest(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE, id);
        }


        public static HttpResponseMessage TryGetForTest(this HttpClient client,
            DbType testDbType, string testDb, params object[] id) {
            Uri getUri = client.BaseAddress.At(id);
            return TryGetForTest(client, testDbType, testDb,
                getUri, getUri, client.BaseAddress);
        }

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


        public static HttpResponseMessage TryGetForTestMultiple(this HttpClient client) {

            return TryGetForTestMultiple(client, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress, client.BaseAddress);
        }

        public static HttpResponseMessage TryGetForTestMultiple(this HttpClient client,
            DbType testDbType, string testDb) {

            return TryGetForTestMultiple(client, testDbType, testDb,
                client.BaseAddress, client.BaseAddress);
        }


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


