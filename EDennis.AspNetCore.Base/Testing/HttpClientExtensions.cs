using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Collections.Generic;
using EDennis.AspNetCore.Base.Web;

using static EDennis.AspNetCore.Base.Web.TestingActionFilter;
using EDennis.AspNetCore.Base.EntityFramework;


namespace EDennis.AspNetCore.Base.Testing {

    public static class HttpClientExtensions {

        #region PostAndGet

        public static T PostAndGet<T>(this HttpClient client,
            T obj, params object[] id) {
            return PostAndGet(client, obj, DbType.InMemory,
                DEFAULT_NAMED_INSTANCE, id);
        }


        public static T PostAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {

            Uri getUri = client.BaseAddress.At(id);
            return PostAndGet(client, obj, testDbType, testDb, getUri, null, null);
        }

        public static T PostAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri, Uri postUri = null, Uri headUri = null) {

            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var json = JToken.FromObject(obj).ToString();
            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = postUri,
                Content = new BodyContent<T>(obj)
            };
            msg.UseTestDbHeader(testDbType, testDb);
            var response = client.SendAsync(msg).Result;
            var result = response.GetObject<T>();

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            T content = response.GetObject<T>();

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            return content;
        }


        public static List<T> PostAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb, Uri getUri) {
            return PostAndGetMultiple(client, obj, testDbType, testDb,
                getUri, client.BaseAddress, client.BaseAddress);
        }


        public static List<T> PostAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri = null, Uri postUri = null, Uri headUri = null) {

            if (getUri == null)
                getUri = client.BaseAddress;
            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = postUri,
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
        #region TryPostAndGet

        public static List<HttpResponseMessage> TryPostAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {

            Uri getUri = client.BaseAddress.At(id);
            return TryPostAndGet(client, obj, testDbType, testDb, getUri, null, null);
        }

        public static List<HttpResponseMessage> TryPostAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri, Uri postUri = null, Uri headUri = null) {

            var responses = new List<HttpResponseMessage>();

            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = client.BaseAddress,
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


        public static List<HttpResponseMessage> TryPostAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb, Uri getUri) {
            return TryPostAndGetMultiple(client, obj, testDbType, testDb,
                getUri, client.BaseAddress, client.BaseAddress);
        }


        public static List<HttpResponseMessage> TryPostAndGetMultiple<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri getUri = null, Uri postUri = null, Uri headUri = null) {

            var responses = new List<HttpResponseMessage>();

            if (getUri == null)
                getUri = client.BaseAddress;
            if (postUri == null)
                postUri = client.BaseAddress;
            if (headUri == null)
                headUri = client.BaseAddress;

            var msg = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = postUri,
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
        #region PutAndGet

        public static T PutAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            params object[] id) {
            Uri putUri = client.BaseAddress.At(id);
            return PutAndGet(client, obj, testDbType, testDb,
                putUri, putUri, putUri);
        }

        public static T PutAndGet<T>(this HttpClient client,
            T obj, DbType testDbType, string testDb,
            Uri putUri, Uri getUri = null, Uri headUri = null) {

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
            var response = client.SendAsync(msg).Result;

            msg = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = getUri,
            };
            msg.UseTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            T content = response.Content.ReadAsAsync<T>().Result;

            msg = new HttpRequestMessage {
                Method = HttpMethod.Head,
                RequestUri = headUri
            };
            msg.DropTestDbHeader(testDbType, testDb);
            response = client.SendAsync(msg).Result;

            return content;
        }


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


