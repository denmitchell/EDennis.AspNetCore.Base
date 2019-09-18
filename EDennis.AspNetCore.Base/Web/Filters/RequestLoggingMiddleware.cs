using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{

    public class RequestLoggingMiddleware
    {
        readonly RequestDelegate _next;
        readonly RequestLoggingOptions _options;

        private const object NA = null;

        public RequestLoggingMiddleware(RequestDelegate next,
            IOptions<RequestLoggingOptions> options) {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
            _options = options?.Value;
        }


        public async Task Invoke(HttpContext httpContext,
            ILogger<RequestLoggingMiddleware> logger) {

            if (!httpContext.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                #region Request

                var req = httpContext.Request;
                var resp = httpContext.Response;

                var url = $"{req.Scheme}://{req.Host}{req.Path}";

                string requestBody = "";
                string responseBody = "";

                req.EnableBuffering(bufferThreshold: 1024 * 1000);

                if (_options.LogRequestBody && req.ContentLength > 0)
                    requestBody = await ToText(req.Body, req.ContentLength, true);

                var originalResponseBody = resp.Body;


                #endregion


                try {
                    using (var responseBodyMem = new MemoryStream()) {
                        //...and use that for the temporary response body
                        resp.Body = responseBodyMem;

                        await _next(httpContext);

                        if (_options.LogResponseBody)
                            responseBody = await ToText(resp.Body,1024*1000, true);

                        //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                        await responseBodyMem.CopyToAsync(originalResponseBody);
                    }

                } catch (Exception ex) {
                    var RequestLog = new RequestLog(_options, httpContext, requestBody, responseBody, ex);

                    logger.Log(LogLevel.Error, "Url: {Url}, QueryString: {QueryString}, Method: {Method}, StatusCode: {StatusCode}, Headers: {Headers}, Claims: {Claims}, RequestBody: {RequestBody:lj}, ResponseBody: {ResponseBody:lj}, ExceptionMessage: {ExceptionMessage}, StackTrace: {StackTrace}",
                        RequestLog.Url, RequestLog.QueryString, RequestLog.Method, RequestLog.StatusCode, RequestLog.Headers, RequestLog.Claims, RequestLog.RequestBody, RequestLog.ResponseBody, RequestLog.ExceptionMessage, RequestLog.StackTrace);
                    throw ex;
                }
                #region Response

                var thresholdStatusCode = _options.MinimumHttpStatusCode;

                var statusCode = httpContext.Response.StatusCode;

                if (statusCode >= thresholdStatusCode) {

                    var logLevel = (statusCode >= 500)
                        ? LogLevel.Error
                        : (statusCode >= 400)
                        ? LogLevel.Warning
                        : LogLevel.Information;

                    var RequestLog = new RequestLog(_options, httpContext, requestBody, responseBody);

                    logger.Log(logLevel, "Url: {Url}, QueryString: {QueryString}, Method: {Method}, StatusCode: {StatusCode}, Headers: {Headers}, Claims: {Claims}, RequestBody: {RequestBody:lj}, ResponseBody: {ResponseBody:ly}",
                        RequestLog.Url, RequestLog.QueryString, RequestLog.Method, RequestLog.StatusCode, RequestLog.Headers, RequestLog.Claims, RequestLog.RequestBody, RequestLog.ResponseBody);

                }
                #endregion

            }
        }

        private async Task<dynamic> ToText(Stream stream, long? size, bool rewind) {
            string text = "";
            //try {
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(
                       stream,
                       encoding: Encoding.UTF8,
                       detectEncodingFromByteOrderMarks: false,
                       bufferSize: Convert.ToInt32(size),
                       leaveOpen: rewind)) {
                    text = await reader.ReadToEndAsync();
                if (rewind)
                    stream.Seek(0, SeekOrigin.Begin);
                }
            //} catch { }
            return text;
        }


    }


    public static class RequestLoggingMiddlewareExtensions
    {
        public static IServiceCollection AddRequestLogging(this IServiceCollection service,
            Action<RequestLoggingOptions> options = default) {

            options = options ?? (opts => { });

            service.Configure(options);
            return service;
        }

        public static IApplicationBuilder UseRequestLogging(
            this IApplicationBuilder app) {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }



    }

    public class RequestLogFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat (Type formatType) {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object obj, IFormatProvider provider) {
            if (obj.GetType().Name == "RequestLog")
                return ((RequestLog)obj).ToString();

            if (obj is IFormattable)
                return ((IFormattable)obj).ToString(format, CultureInfo.CurrentCulture);
            else
                return obj.ToString();
        }
    }

    public class RequestLog {
        private RequestLoggingOptions _requestLoggingOptions;
        private HttpContext _httpContext;
        private string _requestBody;
        private string _responseBody;
        private Exception _exception;

        public string Url { get; set; }
        public string QueryString { get; set; }
        public string Method {get; set;}
        public int StatusCode { get; set;}
        public Dictionary<string, string> Headers { get; set;}
        public Dictionary<string,string> Claims { get; set;}
        public string RequestBody { get; set;}
        public string ResponseBody { get; set;}
        public string ExceptionMessage { get; set;}
        public string StackTrace { get; set;}


        public RequestLog(RequestLoggingOptions requestLoggingOptions, 
            HttpContext httpContext, string requestBody = null,
            string responseBody = null,
            Exception exception = null
            ) {
            _requestLoggingOptions = requestLoggingOptions;
            _httpContext = httpContext;
            _requestBody = requestBody;
            _responseBody = responseBody;
            _exception = exception;

            Build();
        }

        public void Build() {

            var req = _httpContext.Request;
            var resp = _httpContext.Response;

            QueryString = req.QueryString.ToString();
            Method = req.Method;
            Url = $"{req.Scheme}://{req.Host}{req.Path}";

            StatusCode = resp.StatusCode;

            if(_requestLoggingOptions.LogRequestBody)
                RequestBody = _requestBody;
            if (_requestLoggingOptions.LogResponseBody)
                ResponseBody = _responseBody;

            if (_requestLoggingOptions.LogExceptionMessage)
                ExceptionMessage = _exception?.Message;
            if (_requestLoggingOptions.LogStackTrace)
                StackTrace = _exception?.StackTrace;


            if (_requestLoggingOptions.LogHeaders) {
                Headers = new Dictionary<string, string>();
                foreach (var header in req.Headers)
                    Headers.Add(header.Key, string.Join(header.Value,','));
            }


            if (_requestLoggingOptions.LogUserClaims && _httpContext?.User?.Claims != null
                && _httpContext.User.Claims.Count() > 0) {
                foreach (var claim in _httpContext.User.Claims) {
                    if (Claims.ContainsKey(claim.Type))
                        Claims[claim.Type] += $",{claim.Value}";
                    else
                        Claims.Add(claim.Type, claim.Value);
                }
            }

        }
    }


}
