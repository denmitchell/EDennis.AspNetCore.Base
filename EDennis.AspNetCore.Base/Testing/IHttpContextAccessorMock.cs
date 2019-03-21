using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json.Linq;

namespace EDennis.AspNetCore.Base.Testing{

    public class IHttpContextAccessorMock : Mock<IHttpContextAccessor> {

        public void SetupUser(string userName) {
            SetupGet(x => x.HttpContext.Request.Headers["X-User"]).Returns(userName);
        }
        public void SetupClientTrace(params string[] clientTrace ) {
            var vals = new StringValues();
            foreach(var c in clientTrace) {
                vals = StringValues.Concat(vals, c);
            }
            SetupGet(x => x.HttpContext.Request.Headers["X-ClientTrace"]).Returns(vals);
        }

        public void SetupTestingInstance(string testInstance) {
            SetupGet(x => x.HttpContext.Request.Headers["X-Testing-UseInMemory"]).Returns(testInstance);
        }


        public void SetupHeaders(Dictionary<string,string> headers) {
            var dict = new HeaderDictionary();
            foreach (var header in headers) {
                dict.Add(header.Key, new StringValues(header.Value));
            }
            SetupGet(x => x.HttpContext.Request.Headers).Returns(dict);
        }

        public void SetupQueryString(IEnumerable<KeyValuePair<string,string>> items) {
            var queryString = QueryString.Create(items);
            SetupGet(x => x.HttpContext.Request.QueryString).Returns(queryString);
        }

        public void SetupBody<T>(T body) {
            SetupGet(x => x.HttpContext.Request.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(JToken.FromObject(body).ToString())));
        }

        public void SetupItems<T>(Dictionary<object,object> items) {
            SetupGet(x => x.HttpContext.Items).Returns(items);
        }


    }
}
