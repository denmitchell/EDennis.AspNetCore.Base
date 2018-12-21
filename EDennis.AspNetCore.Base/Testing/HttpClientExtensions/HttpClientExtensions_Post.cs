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
        /// POSTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetForTest uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the POSTed object</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PostAndGetForTest<T>(this HttpClient client,
            T obj, params object[] id) {
            return PostAndGetForTest(client, obj, DbType.InMemory,
                DEFAULT_NAMED_INSTANCE, id);
        }

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This version of PutAndGetForTest can use either
        /// an in-memory database or a database with an
        /// open transaction that can be rolled back.
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the POSTed object</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PostAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {

            //build the URL for the GET call
            Uri getUri = client.BaseAddress.At(id);

            return PostAndGetForTest(client, obj, testDbType, testDb, getUri, null, null);
        }

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
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
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>the GETed object, which should be equivalent to the POSTed object</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PostAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri, Uri postUri = null, Uri headUri = null) {

            //use the HttpClient's base address as the default for POSTs and HEADs
            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

           //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = postUri,
                Content = new BodyContent<T>(obj) //serialize the object
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //POST the message
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
        /// POSTs an object to the HttpClient's base address,
        /// retrieves a list of object via GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the POST, GET and HEAD URLs use the base URL for the HttpClient
        /// NOTE: this version of the method assumes that the
        /// test DB type is in-memory and the DB name is the default
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PostAndGetMultipleForTest<T>(this HttpClient client, T obj) {
            return PostAndGetMultipleForTest(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress, client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves a list of object via GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the POST, GET and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PostAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb) {
            return PostAndGetMultipleForTest(client, obj, testDbType, testDb,
                client.BaseAddress, client.BaseAddress, client.BaseAddress);
        }


        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves a list of object via GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>List of GETed objects</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PostAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri = null, Uri postUri = null, Uri headUri = null) {

            //use the HttpClient's base address as the default for GETs, POSTs, HEADs
            if (getUri == null)
                getUri = client.BaseAddress;
            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = postUri,
                Content = new BodyContent<T>(obj) //serialize the object
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //POST the message
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
        #region TryPostAndGetForTest

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of the method uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetForTest<T>(this HttpClient client,
            T obj, params object[] id) {

            Uri getUri = client.BaseAddress.At(id);
            return TryPostAndGetForTest(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE, getUri, null, null);
        }

        
        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {

            Uri getUri = client.BaseAddress.At(id);
            return TryPostAndGetForTest(client, obj, testDbType, testDb, getUri, null, null);
        }

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri, Uri postUri = null, Uri headUri = null) {

            //initialize the to-be-returned list of responses
            var responses = new List<HttpResponseMessage>();

            //use the HttpClient's base address as the default for POSTs and HEADs
            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = client.BaseAddress,
                Content = new BodyContent<T>(obj) //serialize the object
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //POST the message and add the result to the to-be-returned list
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

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the POST, GET and HEAD URLs use the base URL for the HttpClient
        /// NOTE: this version of the method assumes that the
        /// test DB type is in-memory and the DB name is the default
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetMultipleForTest<T>(this HttpClient client,
            T obj) {
            return TryPostAndGetMultipleForTest(client, obj, DbType.InMemory, 
                DEFAULT_NAMED_INSTANCE,client.BaseAddress,
                client.BaseAddress, client.BaseAddress);
        }


        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb, Uri getUri) {
            return TryPostAndGetMultipleForTest(client, obj, testDbType, testDb,
                getUri, client.BaseAddress, client.BaseAddress);
        }

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGetForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultipleForTest{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetMultipleForTest<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri = null, Uri postUri = null, Uri headUri = null) {

            //initialize the to-be-returned list of responses
            var responses = new List<HttpResponseMessage>();

            //use the HttpClient's base address as the default for GETs, POSTs and HEADs
            if (getUri == null)
                getUri = client.BaseAddress;
            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            //build the request message object for the POST
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = postUri,
                Content = new BodyContent<T>(obj)
            };

            //build the X-Testing- header
            msg.UseTestDbHeader(testDbType, testDb);

            //POST the message and add the result to the to-be-returned list
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


