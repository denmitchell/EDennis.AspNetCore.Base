using Microsoft.AspNetCore.Http;
using System.IO;

namespace EDennis.AspNetCore.Base.Testing {



    public class HttpRequestData {


        public HttpRequestData(HttpRequest request, bool enableBuffering = false){

            Method = request.Method;
            Headers = request.Headers;
            Scheme = request.Scheme;
            IsHttps = request.IsHttps;
            Host = request.Host;
            PathBase = request.PathBase;
            QueryString = request.QueryString;
            Query = request.Query;
            Protocol = request.Protocol;
            Cookies = request.Cookies;
            ContentLength = request.ContentLength;
            ContentType = request.ContentType;
            HasFormContentType = request.HasFormContentType;
            Form = request.Form;

            if (enableBuffering) {
                request.EnableBuffering();
            }

            StreamReader reader = new StreamReader(request.Body);
            Body = reader.ReadToEnd();

            if (enableBuffering) {
                request.Body.Position = 0;
            }

        }


        public string Method { get; set; }
        public IHeaderDictionary Headers { get; set; }

        public string Scheme { get; set; }
        public bool IsHttps { get; set; }
        public HostString Host { get; set; }
        public PathString PathBase { get; set; }
        public PathString Path { get; set; }
        public QueryString QueryString { get; set; }
        public IQueryCollection Query { get; set; }
        public string Protocol { get; set; }
        public IRequestCookieCollection Cookies { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public bool HasFormContentType { get; set; }
        public IFormCollection Form { get; set; }

    }
}
