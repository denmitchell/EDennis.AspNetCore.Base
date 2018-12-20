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
    public static class HttpClientExtensions {

        #region PostAndGet

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PostAndGet uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the POSTed object</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PostAndGet<T>(this HttpClient client,
            T obj, params object[] id) {
            return PostAndGet(client, obj, DbType.InMemory,
                DEFAULT_NAMED_INSTANCE, id);
        }


        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This version of PostAndGet can use either
        /// an in-memory database or a database with an
        /// open transaction that can be rolled back.
        /// NOTE: this version of the method assumes that
        /// the POST and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to POST and GET</typeparam>
        /// <param name="client">The HttpClient used to POST and GET</param>
        /// <param name="obj">The object to POST</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the POSTed object</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PostAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {

            //build the URL for the GET call
            Uri getUri = client.BaseAddress.At(id);

            return PostAndGet(client, obj, testDbType, testDb, getUri, null, null);
        }

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PostAndGet (a) can use either
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
        /// <returns>the GETed object, which should be equivalent to the POSTed object</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PostAndGet<T>(this HttpClient client,
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
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PostAndGetMultiple<T>(this HttpClient client, T obj) {
            return PostAndGetMultiple(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
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
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <returns>the GETed list of objects</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PostAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb) {
            return PostAndGetMultiple(client, obj, testDbType, testDb,
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
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>List of GETed objects</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PostAndGetMultiple<T>(this HttpClient client,
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
        #region TryPostAndGet

        /// <summary>
        /// POSTs an object to the HttpClient's base address,
        /// attempts to retrieves the object via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns a list of HttpResponseMessage objects
        /// -- one for the POST and one for the GET.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PostAndGet uses an 
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
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGet<T>(this HttpClient client,
            T obj, params object[] id) {

            Uri getUri = client.BaseAddress.At(id);
            return TryPostAndGet(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE, getUri, null, null);
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
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {

            Uri getUri = client.BaseAddress.At(id);
            return TryPostAndGet(client, obj, testDbType, testDb, getUri, null, null);
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
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGet<T>(this HttpClient client,
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
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetMultiple<T>(this HttpClient client,
            T obj) {
            return TryPostAndGetMultiple(client, obj, DbType.InMemory, 
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
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb, Uri getUri) {
            return TryPostAndGetMultiple(client, obj, testDbType, testDb,
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
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="getUri">The URL for the GET request</param>
        /// <param name="postUri">The URL for the POST request</param>
        /// <param name="headUri">The URL for the HEAD request</param>
        /// <returns>a list of HttpResponseMessage objects, which contain
        /// status codes and return objects (when relevant) for the POST and GET</returns>
        /// <see cref="PostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="PostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPostAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string)"/>
        /// <see cref="TryPostAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<HttpResponseMessage> TryPostAndGetMultiple<T>(this HttpClient client,
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
        #region PutAndGet

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGet uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the PUT and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the PUTted object</returns>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PutAndGet<T>(this HttpClient client,
            T obj, params object[] id) {
            return PutAndGet(client, obj, DbType.InMemory,
                DEFAULT_NAMED_INSTANCE, id);
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves the object via a distinct GET, sends
        /// a HEAD request to drop the testing database,
        /// and returns the GETed object.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGet uses an 
        /// in-memory database with a default name
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name of the test database (often the ConnectionString key)</param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns>the GETed object, which should be equivalent to the PUTted object</returns>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static T PutAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {
            Uri putUri = client.BaseAddress.At(id);
            return PutAndGet(client, obj, testDbType, testDb,
                putUri, putUri, putUri);
        }

        /// <summary>
        /// PUTs an object to the HttpClient's base address,
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
        /// <returns></returns>
        public static T PutAndGet<T>(this HttpClient client,
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
        /// NOTE: This version of PutAndGetMultiple uses an 
        /// in-memory database with a default name
        /// NOTE: this version of the method assumes that
        /// the PUT and HEAD URLs use the base URL for the HttpClient
        /// </summary>
        /// <typeparam name="T">The object type to PUT and GET</typeparam>
        /// <param name="client">The HttpClient used to PUT and GET</param>
        /// <param name="obj">The object to PUT</param>
        /// <param name="getUri"></param>
        /// <param name="id">The expected ID of the object to retrieve</param>
        /// <returns></returns>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PutAndGetMultiple<T>(this HttpClient client,
            T obj, Uri getUri, params object[] id) {

            return PutAndGetMultiple(client, obj, DbType.InMemory, DEFAULT_NAMED_INSTANCE,
                client.BaseAddress.At(id), getUri, client.BaseAddress, client.BaseAddress);
        }

        
        /// <summary>
        /// PUTs an object to the HttpClient's base address,
        /// retrieves a list of objects via a distinct GET, 
        /// sends a HEAD request to drop the testing database,
        /// and returns the GETed list of objects.
        /// NOTE: This method assumes that HttpClient's 
        /// default headers include an X-Testing- header
        /// NOTE: This version of PutAndGetMultiple uses an 
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
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="PutAndGet{T}(HttpClient, T, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="PutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, object[])"/>
        /// <see cref="TryPutAndGet{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, object[])"/>
        /// <see cref="TryPutAndGetMultiple{T}(HttpClient, T, DbType, string, Uri, Uri, Uri)"/>
        public static List<T> PutAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri, params object[] id) {

            return PutAndGetMultiple(client, obj, testDbType, testDb,
                client.BaseAddress.At(id), getUri, client.BaseAddress, client.BaseAddress);
        }


        public static List<T> PutAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };
            msg.UseTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            List<T> content = response.Content.ReadAsAsync<List<T>>().Result;

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            return content;
        }

        #endregion
        #region TryPutAndGet

        public static List<HttpResponseMessage> TryPutAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {
            Uri putUri = client.BaseAddress.At(id);
            return TryPutAndGet(client, obj, testDbType, testDb,
                putUri, putUri, putUri);
        }

        public static List<HttpResponseMessage> TryPutAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            var responses = new List<HttpResponseMessage>();

            if (getUri == null)
                getUri = putUri;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };
            msg.UseTestDbHeader(testDbType, testDb);
            responses.Add(client.SendAsync(msg).Result);

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            responses.Add(client.SendAsync(msg).Result);

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;

            return responses;
        }


        public static List<T> TryPutAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri, params object[] id) {

            return TryPutAndGetMultiple(client, obj, testDbType, testDb,
                client.BaseAddress.At(id), getUri, client.BaseAddress, client.BaseAddress);
        }


        public static List<HttpResponseMessage> TryPutAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

            var responses = new List<HttpResponseMessage>();

            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = putUri,
                Content = new BodyContent<T>(obj)
            };
            msg.UseTestDbHeader(testDbType, testDb);
            responses.Add(client.SendAsync(msg).Result);

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            responses.Add(client.SendAsync(msg).Result);

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;

            return responses;
        }

        #endregion
        #region DeleteAndGet

        public static List<T> DeleteAndGetMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri getUri, params object[] id) {

            return DeleteAndGetMultiple<T>(client, testDbType, testDb,
                client.BaseAddress.At(id), getUri, client.BaseAddress);
        }


        public static List<T> DeleteAndGetMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri deleteUri, Uri getUri = null, Uri headUri = null) {

            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = deleteUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            List<T> content = response.Content.ReadAsAsync<List<T>>().Result;

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            return content;
        }

        #endregion
        #region TryDeleteAndGet

        public static List<HttpResponseMessage> TryDeleteAndGetMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri getUri, params object[] id) {

            return TryDeleteAndGetMultiple<T>(client, testDbType, testDb,
                client.BaseAddress.At(id), getUri, client.BaseAddress);
        }


        public static List<HttpResponseMessage> TryDeleteAndGetMultiple<T>(this HttpClient client,
            DbType testDbType, string testDb,
            Uri deleteUri, Uri getUri = null, Uri headUri = null) {

            var responses = new List<HttpResponseMessage>();

            if (getUri == null)
                getUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = deleteUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            responses.Add(client.SendAsync(msg).Result);

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            responses.Add(client.SendAsync(msg).Result);

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;

            return responses;
        }

        #endregion
        #region Helper Methods


        /// <summary>
        /// Explicitly adds headers from the default
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="testDbType"></param>
        /// <param name="testDb"></param>
        private static void UseTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            if (testDbType == DbType.InMemory) {
                var key = HDR_USE_INMEMORY;
                msg.Headers.Add(key, testDb);
            }
        }

        private static void DropTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            if (testDbType == DbType.InMemory) {
                var key = HDR_DROP_INMEMORY;
                msg.Headers.Add(key, testDb);
            }
        }

        #endregion


    }


}


